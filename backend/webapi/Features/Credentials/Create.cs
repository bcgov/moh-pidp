namespace Pidp.Features.Credentials;

using DomainResults.Common;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.Models;
using NodaTime;
using System.Security.Claims;
using System.Text.Json.Serialization;

using Pidp.Data;
using Pidp.Extensions;
using Pidp.Features.Discovery;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.BCProvider;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models;
using Pidp.Models.DomainEvents;
using Pidp.Models.Lookups;
using Pidp.Infrastructure.HttpClients.Keycloak;

public class Create
{
    public class Command : ICommand<IDomainResult<int>>
    {
        [JsonIgnore]
        public Guid CredentialLinkToken { get; set; }
        [JsonIgnore]
        public ClaimsPrincipal User { get; set; } = new();
    }

    public class CommandHandler : ICommandHandler<Command, IDomainResult<int>>
    {
        private readonly IClock clock;
        private readonly ILogger<CommandHandler> logger;
        private readonly PidpDbContext context;

        public CommandHandler(
            IClock clock,
            ILogger<CommandHandler> logger,
            PidpDbContext context)
        {
            this.clock = clock;
            this.logger = logger;
            this.context = context;
        }

        public async Task<IDomainResult<int>> HandleAsync(Command command)
        {
            var userId = command.User.GetUserId();
            var userIdentityProvider = command.User.GetIdentityProvider();
            var userIdpId = command.User.GetIdpId();
            if (userId == Guid.Empty
                || string.IsNullOrWhiteSpace(userIdentityProvider)
                || string.IsNullOrWhiteSpace(userIdpId))
            {
                this.logger.LogUserError(userId, userIdentityProvider, userIdpId);
                return DomainResult.Failed<int>();
            }

            var ticket = await this.context.CredentialLinkTickets
                .SingleOrDefaultAsync(ticket => ticket.Token == command.CredentialLinkToken
                    && !ticket.Claimed);

            if (ticket == null)
            {
                this.logger.LogTicketNotFound(command.CredentialLinkToken);
                return DomainResult.NotFound<int>();
            }
            if (ticket.LinkToIdentityProvider != userIdentityProvider)
            {
                this.logger.LogTicketIdpError(ticket.Id, ticket.LinkToIdentityProvider, userIdentityProvider);
                return DomainResult.Failed<int>();
            }
            if (ticket.ExpiresAt < this.clock.GetCurrentInstant())
            {
                this.logger.LogTicketExpired(ticket.Id);
                return DomainResult.Failed<int>();
            }

#pragma warning disable CA1304 // ToLower() is Locale Dependant
            var existingCredential = await this.context.Credentials
                .Where(credential => credential.UserId == userId
                    || (credential.IdentityProvider == userIdentityProvider
                        && credential.IdpId!.ToLower() == userIdpId.ToLower()))
                .SingleOrDefaultAsync();
#pragma warning restore CA1304

            if (existingCredential != null)
            {
                this.logger.LogCredentialAlreadyExists(ticket.Id, existingCredential.Id);
                return DomainResult.Failed<int>();
            }

            var credential = new Credential
            {
                PartyId = ticket.PartyId,
                UserId = userId,
                IdentityProvider = userIdentityProvider,
                IdpId = userIdpId
            };
            credential.DomainEvents.Add(new CredentialLinked(credential, command.User));
            credential.DomainEvents.Add(new CollegeLicenceUpdated(credential.PartyId));
            this.context.Credentials.Add(credential);

            ticket.Claimed = true;

            await this.context.SaveChangesAsync();

            return DomainResult.Success(ticket.PartyId);
        }
    }

    public class BCProviderUpdateAttributesHandler : INotificationHandler<CredentialLinked>
    {
        private readonly IBCProviderClient bcProviderClient;
        private readonly IPlrClient plrClient;
        private readonly PidpDbContext context;
        private readonly string bcProviderClientId;

        public BCProviderUpdateAttributesHandler(
            IBCProviderClient bcProviderClient,
            IPlrClient plrClient,
            PidpConfiguration config,
            PidpDbContext context)
        {
            this.bcProviderClient = bcProviderClient;
            this.plrClient = plrClient;
            this.context = context;
            this.bcProviderClientId = config.BCProviderClient.ClientId;
        }

        public async Task Handle(CredentialLinked notification, CancellationToken cancellationToken)
        {
            var credential = notification.Credential;

            if (credential.IdentityProvider != IdentityProviders.BCProvider
                || string.IsNullOrWhiteSpace(credential.IdpId))
            {
                return;
            }

            var party = await this.context.Parties
                .Where(party => party.Id == credential.PartyId)
                .Select(party => new
                {
                    party.FirstName,
                    party.LastName,
                    party.Cpn,
                    party.Email,
                    Hpdid = party.Credentials
                        .Select(cred => cred.Hpdid)
                        .Single(hpdid => hpdid != null),
                    UaaAgreementDate = party.AccessRequests
                        .Where(request => request.AccessTypeCode == AccessTypeCode.UserAccessAgreement)
                        .Select(request => request.RequestedOn)
                        .SingleOrDefault()
                })
                .SingleAsync(cancellationToken);

            var plrStanding = await this.plrClient.GetStandingsDigestAsync(party.Cpn);
            var endorsingCpns = await this.context.ActiveEndorsingParties(credential.PartyId)
                .Select(party => party.Cpn)
                .ToListAsync(cancellationToken);
            var endorsementPlrDigest = await this.plrClient.GetAggregateStandingsDigestAsync(endorsingCpns);

            var attributes = new BCProviderAttributes(this.bcProviderClientId)
                .SetHpdid(party.Hpdid!)
                .SetLoa(3)
                .SetPidpEmail(party.Email!)
                .SetIsRnp(plrStanding.With(ProviderRoleType.RegisteredNursePractitioner).HasGoodStanding)
                .SetIsMd(plrStanding.With(ProviderRoleType.MedicalDoctor).HasGoodStanding)
                .SetIsMoa(!plrStanding.HasGoodStanding && endorsementPlrDigest.HasGoodStanding)
                .SetEndorserData(endorsementPlrDigest
                    .WithGoodStanding()
                    .With(BCProviderAttributes.EndorserDataEligibleIdentifierTypes)
                    .Cpns);

            if (party.Cpn != null)
            {
                attributes.SetCpn(party.Cpn);
            }
            if (party.UaaAgreementDate != default)
            {
                attributes.SetUaaDate(party.UaaAgreementDate.ToDateTimeOffset());
            }

            var user = new User
            {
                GivenName = party.FirstName,
                Surname = party.LastName,
                AdditionalData = attributes.AsAdditionalData()
            };

            await this.bcProviderClient.UpdateUser(credential.IdpId, user);
        }
    }

    public class UpdateBCServicesCardAttributesHandler : INotificationHandler<CredentialLinked>
    {
        private readonly IKeycloakAdministrationClient keycloakClient;
        private readonly PidpDbContext context;

        public UpdateBCServicesCardAttributesHandler(
            IKeycloakAdministrationClient keycloakClient,
            PidpDbContext context)
        {
            this.keycloakClient = keycloakClient;
            this.context = context;
        }

        public async Task Handle(CredentialLinked notification, CancellationToken cancellationToken)
        {
            var newCredential = notification.Credential;

            var party = await this.context.Parties
                .Include(party => party.Credentials)
                .SingleAsync(party => party.Id == newCredential.PartyId, cancellationToken);

            if (newCredential.IdentityProvider == IdentityProviders.BCServicesCard)
            {
                party.Birthdate = notification.User.GetBirthdate();
                await party.GenerateOpId(this.context);
                await this.context.SaveChangesAsync(cancellationToken);

                foreach (var credential in party.Credentials)
                {
                    await this.keycloakClient.UpdateUser(credential.UserId, user => user.SetOpId(party.OpId!));
                }
            }
            else
            {
                await this.keycloakClient.UpdateUser(newCredential.UserId, user => user.SetOpId(party.OpId!));
            }
        }
    }
}

public static partial class CredentialCreateLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Error, "User object is missing one or more required properties: User ID = {userId}, IDP = {idp}, IDP ID = {idpId}.")]
    public static partial void LogUserError(this ILogger<Create.CommandHandler> logger, Guid userId, string? idp, string? idpId);

    [LoggerMessage(2, LogLevel.Error, "A unclaimed Credential Link Ticket with token {credentialLinkToken} was not found.")]
    public static partial void LogTicketNotFound(this ILogger<Create.CommandHandler> logger, Guid credentialLinkToken);

    [LoggerMessage(3, LogLevel.Error, "Credential Link Ticket {credentialLinkTicketId} is expired.")]
    public static partial void LogTicketExpired(this ILogger<Create.CommandHandler> logger, int credentialLinkTicketId);

    [LoggerMessage(4, LogLevel.Error, "Credential Link Ticket {credentialLinkTicketId} expected to link to IDP {expectedIdp}, user had IDP {actualIdp} instead.")]
    public static partial void LogTicketIdpError(this ILogger<Create.CommandHandler> logger, int credentialLinkTicketId, string expectedIdp, string actualIdp);

    [LoggerMessage(5, LogLevel.Error, "Credential Link Ticket {credentialLinkTicketId} redemption failed, the Credential with ID {existingCredentialId} already exists.")]
    public static partial void LogCredentialAlreadyExists(this ILogger<Create.CommandHandler> logger, int credentialLinkTicketId, int existingCredentialId);
}
