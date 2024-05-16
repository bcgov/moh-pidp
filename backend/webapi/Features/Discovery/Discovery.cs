namespace Pidp.Features.Discovery;

using Microsoft.EntityFrameworkCore;
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
    }

    public class Model
    {
        public enum StatusCode
        {
            Success = 1,
            NewUser,
            NewBCProviderError,
            AccountLinkSuccess,
            AlreadyLinked,
            CredentialExists,
            TicketExpired
        }

        public int? PartyId { get; set; }
        public StatusCode Status { get; set; }
    }

    public class QueryHandler : IQueryHandler<Query, Model>
    {
        private readonly IPlrClient client;
        private readonly PidpDbContext context;

        public QueryHandler(IPlrClient client, PidpDbContext context)
        {
            this.client = client;
            this.context = context;
        }

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
                        && credential.Party.LicenceDeclaration!.CollegeCode != null
                })
                .SingleOrDefaultAsync();

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
