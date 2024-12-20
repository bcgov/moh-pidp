namespace Pidp.Features.Discovery;

using Microsoft.EntityFrameworkCore;
using NodaTime;
using System.Security.Claims;
using System.Text.Json.Serialization;

using Pidp.Data;
using Pidp.Extensions;
using Pidp.Models;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.Plr;

public class Discovery
{
    public class Query : IQuery<Model>
    {
        [JsonIgnore]
        public ClaimsPrincipal User { get; set; } = new();
        [JsonIgnore]
        public Guid? CredentialLinkToken { get; set; }
    }

    public class Model
    {
        public enum StatusCode
        {
            Success = 1,
            NewUser,
            NewBCProviderError,
            AccountLinkInProgress,
            AlreadyLinked,
            CredentialExists,
            TicketExpired,
            AccountLinkingError
        }

        public int? PartyId { get; set; }
        public StatusCode Status { get; set; }
    }

    public class QueryHandler(
        IClock clock,
        ILogger<QueryHandler> logger,
        IPlrClient client,
        PidpDbContext context) : IQueryHandler<Query, Model>
    {
        private readonly IClock clock = clock;
        private readonly ILogger<QueryHandler> logger = logger;
        private readonly IPlrClient client = client;
        private readonly PidpDbContext context = context;

        public async Task<Model> HandleAsync(Query query)
        {
            var idpId = query.User.GetIdpId()?.ToLowerInvariant();

            var data = await this.context.Credentials
#pragma warning disable CA1304 // ToLower() is Locale Dependant
                .Where(credential => credential.UserId == query.User.GetUserId()
                    || (credential.IdentityProvider == query.User.GetIdentityProvider()
                        && credential.IdpId!.ToLower() == idpId))
#pragma warning restore CA1304
                .Select(credential => new
                {
                    Credential = credential,
                    CheckPlr = credential.Party!.Cpn == null
                        && credential.Party.Birthdate != null
                        && credential.Party.LicenceDeclaration!.LicenceNumber != null
                        && credential.Party.LicenceDeclaration!.CollegeCode != null,
                })
                .SingleOrDefaultAsync();

            if (query.CredentialLinkToken != null)
            {
                var model = await this.HandleAcountLinkingDiscovery(query, data?.Credential);
                if (model.Status != Model.StatusCode.AccountLinkInProgress)
                {
                    await this.context.SaveChangesAsync();
                }
                return model;
            }

            if (data == null)
            {
                return new Model
                {
                    Status = query.User.GetIdentityProvider() == IdentityProviders.BCProvider
                        ? Model.StatusCode.NewBCProviderError
                        : Model.StatusCode.NewUser
                };
            }

            await this.HandleUpdatesAsync(data.Credential, data.CheckPlr, query.User);

            return new Model
            {
                PartyId = data.Credential.PartyId,
                Status = Model.StatusCode.Success
            };
        }

        private async Task<Model> HandleAcountLinkingDiscovery(Query query, Credential? credential)
        {
            var ticket = await this.context.CredentialLinkTickets
                .SingleOrDefaultAsync(ticket => ticket.Token == query.CredentialLinkToken
                    && !ticket.Claimed);

            if (ticket == null)
            {
                this.logger.LogTicketNotFound(query.User.GetUserId(), query.CredentialLinkToken!.Value);
                this.context.BusinessEvents.Add(AccountLinkingFailure.CreateTicketNotFound(query.User.GetUserId(), query.CredentialLinkToken.Value, this.clock.GetCurrentInstant()));
                return new Model { Status = Model.StatusCode.AccountLinkingError };
            }

            if (credential != null)
            {
                // Either the Credential is already linked to the Party, or the Credential already exists on a different Party.
                if (credential.PartyId == ticket.PartyId)
                {
                    this.logger.LogCredentialAlreadyLinked(query.User.GetUserId(), ticket.Id, credential.Id);
                    this.context.BusinessEvents.Add(AccountLinkingFailure.CreateCredentialAlreadyLinked(ticket.PartyId, credential.Id, ticket.Id, this.clock.GetCurrentInstant()));
                    return new Model
                    {
                        PartyId = credential.PartyId,
                        Status = Model.StatusCode.AlreadyLinked
                    };
                }
                else
                {
                    this.context.CredentialLinkErrorLogs.Add(new CredentialLinkErrorLog
                    {
                        CredentialLinkTicketId = ticket.Id,
                        ExistingCredentialId = credential.Id
                    });

                    this.logger.LogCredentialAlreadyExists(query.User.GetUserId(), ticket.Id, credential.Id);
                    this.context.BusinessEvents.Add(AccountLinkingFailure.CreateCredentialExists(ticket.PartyId, credential.Id, ticket.Id, this.clock.GetCurrentInstant()));
                    return new Model
                    {
                        PartyId = credential.PartyId,
                        Status = Model.StatusCode.CredentialExists
                    };
                }
            }

            if (ticket.LinkToIdentityProvider != query.User.GetIdentityProvider())
            {
                this.logger.LogTicketIdpError(query.User.GetUserId(), ticket.Id, ticket.LinkToIdentityProvider, query.User.GetIdentityProvider());
                this.context.BusinessEvents.Add(AccountLinkingFailure.CreateWrongIdentityProvider(ticket.PartyId, ticket.Id, query.User.GetIdentityProvider(), this.clock.GetCurrentInstant()));
                return new Model { Status = Model.StatusCode.AccountLinkingError };
            }
            if (ticket.ExpiresAt < this.clock.GetCurrentInstant())
            {
                this.logger.LogTicketExpired(query.User.GetUserId(), ticket.Id);
                this.context.BusinessEvents.Add(AccountLinkingFailure.CreateTicketExpired(ticket.PartyId, ticket.Id, this.clock.GetCurrentInstant()));
                return new Model { Status = Model.StatusCode.TicketExpired };
            }

            return new Model
            {
                Status = Model.StatusCode.AccountLinkInProgress
            };
        }

        private async Task HandleUpdatesAsync(Credential credential, bool checkPlr, ClaimsPrincipal user)
        {
            var saveChanges = false;

            if (checkPlr)
            {
                var party = await this.context.Parties
                    .Include(party => party.Credentials)
                    .Include(party => party.LicenceDeclaration)
                    .SingleAsync(party => party.Id == credential.PartyId);

                await party.HandleLicenceSearch(this.client, this.context);
                if (!string.IsNullOrWhiteSpace(party.Cpn))
                {
                    saveChanges = true;
                }
            }

            // This is to update old non-BCSC records we didn't originally capture the IDP info for.
            // One day, this should be removed entirely once all the records in the DB have IdentityProvider and IdpId (also, those properties can then be made non-nullable).
            // Additionally, we could then find the Credential using only IdentityProvider + IdpId.
            if (credential.IdentityProvider == null
                || credential.IdpId == null)
            {
                credential.IdentityProvider ??= user.GetIdentityProvider();
                credential.IdpId ??= user.GetIdpId();
                saveChanges = true;
            }

            if (saveChanges)
            {
                await this.context.SaveChangesAsync();
            }
        }
    }
}


public static partial class DiscoveryLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Error, "User {userId} Discovery: no unclaimed Credential Link Ticket with token {credentialLinkToken} was found.")]
    public static partial void LogTicketNotFound(this ILogger<Discovery.QueryHandler> logger, Guid userId, Guid credentialLinkToken);

    [LoggerMessage(2, LogLevel.Error, "User {userId} Discovery: Credential Link Ticket {credentialLinkTicketId} is expired.")]
    public static partial void LogTicketExpired(this ILogger<Discovery.QueryHandler> logger, Guid userId, int credentialLinkTicketId);

    [LoggerMessage(3, LogLevel.Error, "User {userId} Discovery: Credential Link Ticket {credentialLinkTicketId} expected to link to IDP {expectedIdp}, user had IDP {actualIdp} instead.")]
    public static partial void LogTicketIdpError(this ILogger<Discovery.QueryHandler> logger, Guid userId, int credentialLinkTicketId, string expectedIdp, string? actualIdp);

    [LoggerMessage(4, LogLevel.Error, "User {userId} Discovery: Credential Link Ticket {credentialLinkTicketId} redemption failed, the new Credential is already linked to the Party. Credential ID {existingCredentialId}.")]
    public static partial void LogCredentialAlreadyLinked(this ILogger<Discovery.QueryHandler> logger, Guid userId, int credentialLinkTicketId, int existingCredentialId);

    [LoggerMessage(5, LogLevel.Error, "User {userId} Discovery: Credential Link Ticket {credentialLinkTicketId} redemption failed, the new Credential already exists on a different party. Credential Id {existingCredentialId}.")]
    public static partial void LogCredentialAlreadyExists(this ILogger<Discovery.QueryHandler> logger, Guid userId, int credentialLinkTicketId, int existingCredentialId);
}
