namespace Pidp.Features.Credentials;

using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

using Pidp.Data;
using Pidp.Extensions;
using Pidp.Models;

public class Index
{
    public class Query : IQuery<List<Model>>
    {
        public ClaimsPrincipal User { get; set; } = new();
    }

    public class Model
    {
        public Guid UserId { get; set; }
        public int PartyId { get; set; }
        public string IdentityProvider { get; set; } = string.Empty;
        public string IdpId { get; set; } = string.Empty;
    }

    public class QueryHandler : IQueryHandler<Query, List<Model>>
    {
        private readonly PidpDbContext context;

        public QueryHandler(PidpDbContext context) => this.context = context;

        public async Task<List<Model>> HandleAsync(Query query)
        {
            var credential = await this.context.Credentials
                .SingleOrDefaultAsync(credential => credential.UserId == query.User.GetUserId()
                    || (credential.IdentityProvider == query.User.GetIdentityProvider()
                        && credential.IdpId == query.User.GetIdpId()));

            if (credential == null)
            {
                return new();
            }

            if (credential.UserId == default)
            {
                await this.UpdateUserId(credential, query.User.GetUserId());
            }
            if (credential.IdentityProvider == null
                || credential.IdpId == null)
            {
                await this.UpdateIncompleteRecord(credential, query.User);
            }

            return new List<Model>
            {
                new Model
                {
                    UserId = credential.UserId,
                    PartyId = credential.PartyId,
                    IdentityProvider = credential.IdentityProvider!,
                    IdpId = credential.IdpId!
                }
            };
        }

        // BC Provider Credentials are created in our app without UserIds (since the user has not logged into Keycloak yet).
        // Update them when we see them.
        private async Task UpdateUserId(Credential credential, Guid userId)
        {
            credential.UserId = userId;
            await this.context.SaveChangesAsync();
        }

        // This is to update old non-BCSC records we didn't originally capture the IDP info for.
        // One day, this should be removed entirely once all the records in the DB have IdentityProvider and IdpId (also, those properties can then be made non-nullable).
        // Additionally, we could then find the Credential using only IdentityProvider + IdpId.
        private async Task UpdateIncompleteRecord(Credential credential, ClaimsPrincipal user)
        {
            credential.IdentityProvider ??= user.GetIdentityProvider();
            credential.IdpId ??= user.GetIdpId();

            await this.context.SaveChangesAsync();
        }
    }
}
