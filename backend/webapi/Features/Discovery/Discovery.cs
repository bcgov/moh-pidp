namespace Pidp.Features.Discovery;

using DomainResults.Common;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

using Pidp.Data;
using Pidp.Extensions;
using Pidp.Infrastructure.Auth;
using Pidp.Models;

public class Discovery
{
    public class Command : ICommand<IDomainResult<int>>
    {
        public ClaimsPrincipal User { get; set; } = new();
    }

    public class CommandHandler : ICommandHandler<Command, IDomainResult<int>>
    {
        private readonly PidpDbContext context;

        public CommandHandler(PidpDbContext context) => this.context = context;

        public async Task<IDomainResult<int>> HandleAsync(Command command)
        {
            var lowerIdpId = command.User.GetIdpId()?.ToLowerInvariant();

            // TODO: consider a more general approach for this; maybe in User.GetIdpId()?
            if (command.User.GetIdentityProvider() == IdentityProviders.BCProvider
                && lowerIdpId != null
                && lowerIdpId.EndsWith("@bcp", StringComparison.InvariantCulture))
            {
                // Keycloak adds "@bcp" at the end of the IDP ID, and so won't match what we have in the DB if we don't trim it.
                lowerIdpId = lowerIdpId[..^4];
            }

#pragma warning disable CA1304 // ToLower() is Locale Dependant
            var credential = await this.context.Credentials
                .SingleOrDefaultAsync(credential => credential.UserId == command.User.GetUserId()
                    || (credential.IdentityProvider == command.User.GetIdentityProvider()
                        && credential.IdpId!.ToLower() == lowerIdpId));
#pragma warning restore CA1304

            if (credential == null)
            {
                return DomainResult.NotFound<int>();
            }

            if (credential.UserId == default)
            {
                await this.UpdateUserId(credential, command.User.GetUserId());
            }
            if (credential.IdentityProvider == null
                || credential.IdpId == null)
            {
                await this.UpdateIncompleteRecord(credential, command.User);
            }

            return DomainResult.Success(credential.PartyId);
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
