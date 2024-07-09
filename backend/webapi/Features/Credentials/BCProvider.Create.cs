namespace Pidp.Features.Credentials;

using DomainResults.Common;
using FluentValidation;
using Flurl;
using HybridModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

using Pidp.Data;
using Pidp.Extensions;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.BCProvider;
using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Infrastructure.HttpClients.Mail;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models;
using Pidp.Models.DomainEvents;
using Pidp.Models.Lookups;
using Pidp.Infrastructure.Services;

public class BCProviderCreate
{
    public class Command : ICommand<IDomainResult<string>>
    {
        [JsonIgnore]
        [HybridBindProperty(Source.Route)]
        public int PartyId { get; set; }
        public string Password { get; set; } = string.Empty;
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            this.RuleFor(x => x.PartyId).GreaterThan(0);
            this.RuleFor(x => x.Password).NotEmpty();
        }
    }

    public class CommandHandler : ICommandHandler<Command, IDomainResult<string>>
    {
        private readonly IBCProviderClient client;
        private readonly IEmailService emailService;
        private readonly IKeycloakAdministrationClient keycloakClient;
        private readonly ILogger<CommandHandler> logger;
        private readonly IPlrClient plrClient;
        private readonly PidpDbContext context;
        private readonly Url keycloakAdministrationUrl;

        public CommandHandler(
            IBCProviderClient client,
            IEmailService emailService,
            IKeycloakAdministrationClient keycloakClient,
            ILogger<CommandHandler> logger,
            IPlrClient plrClient,
            PidpConfiguration config,
            PidpDbContext context)
        {
            this.client = client;
            this.emailService = emailService;
            this.keycloakClient = keycloakClient;
            this.logger = logger;
            this.plrClient = plrClient;
            this.context = context;
            this.keycloakAdministrationUrl = Url.Parse(config.Keycloak.AdministrationUrl);
        }

        public async Task<IDomainResult<string>> HandleAsync(Command command)
        {
            var party = await this.context.Parties
                .Where(party => party.Id == command.PartyId)
                .Select(party => new
                {
                    party.OpId,
                    party.FirstName,
                    party.LastName,
                    party.Cpn,
                    party.Email,
                    HasBCProviderCredential = party.Credentials.Any(credential => credential.IdentityProvider == IdentityProviders.BCProvider),
                    Hpdid = party.Credentials.Select(credential => credential.Hpdid).Single(hpdid => hpdid != null),
                    UaaAgreementDate = party.AccessRequests
                        .Where(request => request.AccessTypeCode == AccessTypeCode.UserAccessAgreement)
                        .Select(request => request.RequestedOn)
                        .SingleOrDefault()
                })
                .SingleAsync();

            if (party.HasBCProviderCredential)
            {
                this.logger.LogPartyHasBCProviderCredential(command.PartyId);
                return DomainResult.Failed<string>();
            }

            if (party.Email == null
                || party.Hpdid == null
                || party.UaaAgreementDate == default)
            {
                this.logger.LogInvalidState(command.PartyId, party);
                return DomainResult.Failed<string>();
            }

            var plrStanding = await this.plrClient.GetStandingsDigestAsync(party.Cpn);

            var endorsingCpns = await this.context.ActiveEndorsingParties(command.PartyId)
                .Select(party => party.Cpn)
                .ToListAsync();
            var endorsingPlrDigest = await this.plrClient.GetAggregateStandingsDigestAsync(endorsingCpns);

            var newUserRep = new NewUserRepresentation
            {
                FirstName = party.FirstName,
                LastName = party.LastName,
                Hpdid = party.Hpdid,
                IsRnp = plrStanding.With(ProviderRoleType.RegisteredNursePractitioner).HasGoodStanding,
                IsMd = plrStanding.With(ProviderRoleType.MedicalDoctor).HasGoodStanding,
                Cpn = party.Cpn,
                Password = command.Password,
                PidpEmail = party.Email,
                UaaDate = party.UaaAgreementDate.ToDateTimeOffset(),
                IsMoa = !plrStanding.HasGoodStanding && endorsingPlrDigest.HasGoodStanding,
                EndorserData = endorsingPlrDigest
                    .WithGoodStanding()
                    .With(BCProviderAttributes.EndorserDataEligibleIdentifierTypes)
                    .Cpns
            };

            var createdUser = await this.client.CreateBCProviderAccount(newUserRep);

            if (createdUser == null || createdUser.UserPrincipalName == null)
            {
                return DomainResult.Failed<string>();
            }

            // TODO: Domain Event! Probably should create this credential now and then Queue the keycloak User creation + updating the UserId
            var userId = await this.CreateKeycloakUser(party.FirstName, party.LastName, createdUser.UserPrincipalName);
            if (userId == null)
            {
                return DomainResult.Failed<string>();
            }
            await this.keycloakClient.UpdateUser(userId.Value, user => user.SetOpId(party.OpId!));

            this.context.Credentials.Add(new Credential
            {
                UserId = userId.Value,
                PartyId = command.PartyId,
                IdpId = createdUser.UserPrincipalName,
                IdentityProvider = IdentityProviders.BCProvider,
                DomainEvents = new List<IDomainEvent> { new CollegeLicenceUpdated(command.PartyId) }
            });

            await this.context.SaveChangesAsync();
            await this.SendBCProviderCreationEmail(party.Email, createdUser.UserPrincipalName);

            return DomainResult.Success(createdUser.UserPrincipalName);
        }

        private async Task<Guid?> CreateKeycloakUser(string firstName, string lastName, string userPrincipalName)
        {
            var newUser = new UserRepresentation
            {
                Enabled = true,
                FirstName = firstName,
                LastName = lastName,
                Username = this.GenerateMohKeycloakUsername(userPrincipalName)
            };
            return await this.keycloakClient.CreateUser(newUser);
        }

        /// <summary>
        /// The expected Ministry of Health Keycloak username for this user. The schema is {IdentityProviderIdentifier}@{IdentityProvider}.
        /// Most of our Credentials come to us from Keycloak and so the username is saved as-is in the column IdpId.
        /// However, we create BC Provider Credentials directly; so the User Principal Name is saved to the database without the suffix.
        /// There are also two inconsistencies with how the MOH Keycloak handles BCP usernames:
        /// 1. The username suffix is @bcp rather than @bcprovider_aad.
        /// 2. This suffix is only added in Test and Production; there is no suffix at all for BCP Users in the Dev Keycloak.
        /// </summary>
        private string GenerateMohKeycloakUsername(string userPrincipalName)
        {
            return userPrincipalName + "@bcp";
        }

        private async Task SendBCProviderCreationEmail(string partyEmail, string userPrincipalName)
        {
            var email = new Email(
                from: EmailService.PidpEmail,
                to: partyEmail,
                subject: "BCProvider Account Creation in OneHealthID Confirmation",
                body: $"You have successfully created a BCProvider account in OneHealthID Service. For your reference, your BCProvider username is {userPrincipalName}. You may now login to OneHealthID Service and access the BCProvider card to update your BCProvider password."
            );

            await this.emailService.SendAsync(email);
        }
    }
}

public static partial class BCProviderCreateLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Information, "Party {partyId} attempted to create a second BC Provider account.")]
    public static partial void LogPartyHasBCProviderCredential(this ILogger<BCProviderCreate.CommandHandler> logger, int partyId);

    [LoggerMessage(2, LogLevel.Error, "Failed to create BC Provider for Party {partyId}, one or more requirements were not met. Party state: {state}.")]
    public static partial void LogInvalidState(this ILogger<BCProviderCreate.CommandHandler> logger, int partyId, object state);

    [LoggerMessage(3, LogLevel.Error, "Error when attempting to create a Keycloak User for Party {partyId} with username {username}.")]
    public static partial void LogKeycloakUserCreationError(this ILogger<BCProviderCreate.CommandHandler> logger, int partyId, string username);
}
