namespace Pidp.Features.Credentials;

using DomainResults.Common;
using FluentValidation;
using HybridModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

using Pidp.Data;
using Pidp.Extensions;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.BCProvider;
using Pidp.Infrastructure.HttpClients.Mail;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models;
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
        private readonly PidpDbContext context;
        private readonly ILogger<CommandHandler> logger;
        private readonly IPlrClient plrClient;

        public CommandHandler(
            IBCProviderClient client,
            IEmailService emailService,
            PidpDbContext context,
            ILogger<CommandHandler> logger,
            IPlrClient plrClient)
        {
            this.client = client;
            this.context = context;
            this.emailService = emailService;
            this.logger = logger;
            this.plrClient = plrClient;
        }

        public async Task<IDomainResult<string>> HandleAsync(Command command)
        {
            var party = await this.context.Parties
                .Where(party => party.Id == command.PartyId)
                .Select(party => new
                {
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

            this.context.Credentials.Add(new Credential
            {
                PartyId = command.PartyId,
                IdpId = createdUser.UserPrincipalName,
                IdentityProvider = IdentityProviders.BCProvider
            });

            await this.context.SaveChangesAsync();
            await this.SendBCProviderCreationEmail(newUserRep.PidpEmail, createdUser.UserPrincipalName);

            return DomainResult.Success(createdUser.UserPrincipalName!);
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
}
