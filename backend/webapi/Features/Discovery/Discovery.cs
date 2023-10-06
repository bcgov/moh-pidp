namespace Pidp.Features.Discovery;

using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json.Serialization;

using Pidp.Data;
using Pidp.Extensions;
using Pidp.Models;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models.Lookups;
using Pidp.Models.DomainEvents;

public class Discovery
{
    public class Query : IQuery<Model>
    {
        [JsonIgnore]
        public ClaimsPrincipal User { get; set; } = new();
    }

    public class Model
    {
        public enum DestinationCode
        {
            NewUser = 1,
            NewBCProvider,
            Demographics,
            UserAccessAgreement,
            Portal,
        }

        public int? PartyId { get; set; }
        public DestinationCode Destination { get; set; }

        [JsonIgnore]
        public bool CheckCookie => this.Destination is DestinationCode.NewBCProvider;
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
                .Select(credential => new DiscoveryData
                {
                    Credential = credential,
                    DemographicsComplete = credential.Party!.Email != null && credential.Party.Phone != null,
                    UaaAccepted = credential.Party.AccessRequests.Any(request => request.AccessTypeCode == AccessTypeCode.UserAccessAgreement),
                    CheckPlr = credential.Party.Cpn == null
                        && credential.Party.Birthdate != null
                        && credential.Party.LicenceDeclaration!.LicenceNumber != null
                        && credential.Party.LicenceDeclaration!.CollegeCode != null
                })
                .SingleOrDefaultAsync();

            if (data == null)
            {
                return new Model
                {
                    Destination = query.User.GetIdentityProvider() == IdentityProviders.BCProvider
                        ? Model.DestinationCode.NewBCProvider
                        : Model.DestinationCode.NewUser
                };
            }

            await this.HandleUpdatesAsync(data, query.User);

            return new Model
            {
                PartyId = data.Credential.PartyId,
                Destination = !data.DemographicsComplete
                    ? Model.DestinationCode.Demographics
                    : data.UaaAccepted
                        ? Model.DestinationCode.Portal
                        : Model.DestinationCode.UserAccessAgreement
            };
        }

        private async Task HandleUpdatesAsync(DiscoveryData data, ClaimsPrincipal user)
        {
            var saveChanges = false;

            if (data.CheckPlr)
            {
                var party = await this.context.Parties
                    .Include(party => party.Credentials)
                    .Include(party => party.LicenceDeclaration)
                    .SingleAsync(party => party.Id == data.Credential.PartyId);

                var cpn = await this.client.FindCpnAsync(party.LicenceDeclaration!.CollegeCode!.Value, party.LicenceDeclaration.LicenceNumber!, party.Birthdate!.Value);
                if (!string.IsNullOrWhiteSpace(cpn))
                {
                    party.Cpn = cpn;
                    party.DomainEvents.Add(new PlrCpnLookupFound(party.Id, party.PrimaryUserId, cpn));
                    saveChanges = true;
                }
            }

            // BC Provider Credentials are created in our app without UserIds (since the user has not logged into Keycloak yet).
            // Update them when we see them.
            if (data.Credential.UserId == default)
            {
                data.Credential.UserId = user.GetUserId();
                saveChanges = true;
            }

            // This is to update old non-BCSC records we didn't originally capture the IDP info for.
            // One day, this should be removed entirely once all the records in the DB have IdentityProvider and IdpId (also, those properties can then be made non-nullable).
            // Additionally, we could then find the Credential using only IdentityProvider + IdpId.
            if (data.Credential.IdentityProvider == null
                || data.Credential.IdpId == null)
            {
                data.Credential.IdentityProvider ??= user.GetIdentityProvider();
                data.Credential.IdpId ??= user.GetIdpId();
                saveChanges = true;
            }

            if (saveChanges)
            {
                await this.context.SaveChangesAsync();
            }
        }

    }

    public class DiscoveryData
    {
        public Credential Credential { get; set; } = new();
        public bool DemographicsComplete { get; set; }
        public bool UaaAccepted { get; set; }
        public bool CheckPlr { get; set; }
    }
}
