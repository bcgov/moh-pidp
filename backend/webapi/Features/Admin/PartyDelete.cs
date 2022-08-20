namespace Pidp.Features.Admin;

using Microsoft.EntityFrameworkCore;

using Pidp.Data;
using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Models;
using Pidp.Models.Lookups;

public class PartyDelete
{
    public class Command : ICommand { }

    public class CommandHandler : ICommandHandler<Command>
    {
        private readonly IKeycloakAdministrationClient client;
        private readonly PidpDbContext context;

        public CommandHandler(IKeycloakAdministrationClient client, PidpDbContext context)
        {
            this.client = client;
            this.context = context;
        }

        public async Task HandleAsync(Command command)
        {
            var cardLastNames = new[]
            {
                "ONE",
                "TWO",
                "THREE",
                "FOUR",
                "FIVE",
                "SIX",
                "SEVEN",
                "EIGHT",
                "NINE",
                "TEN",
                "ELEVEN",
                "TWELVE",
                "THIRTEEN",
                "FOURTEEN",
                "FIFTEEN",
                "SIXTEEN",
                "SEVENTEEN",
                "EIGHTEEN",
                "NINETEEN",
                "TWENTY"
            };

            var parties = await this.context.Parties
                .Include(party => party.AccessRequests)
                .Where(party => party.FirstName == "PIDP"
                    || cardLastNames.Contains(party.LastName))
                .ToListAsync();

            if (!parties.Any())
            {
                return;
            }

            var roleRemover = new RoleRemover(this.client);
            foreach (var party in parties)
            {
                await roleRemover.RemoveClientRoles(party);
            }

            this.context.Parties.RemoveRange(parties);
            await this.context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Remember Keycloak Role representations between users to reduce total number of calls to keycloak.
    /// </summary>
    private class RoleRemover
    {
        private readonly Dictionary<AccessTypeCode, Role?> roleCache;
        private readonly IKeycloakAdministrationClient client;

        public RoleRemover(IKeycloakAdministrationClient client)
        {
            this.client = client;
            this.roleCache = new();
        }

        public async Task RemoveClientRoles(Party party)
        {
            foreach (var access in party.AccessRequests)
            {
                var role = await this.FetchClientRole(access.AccessTypeCode);
                if (role == null)
                {
                    continue;
                }

                if (!await this.client.RemoveClientRole(party.UserId, role))
                {
                    // log failure;
                }
            }
        }

        private async Task<Role?> FetchClientRole(AccessTypeCode accessType)
        {
            if (this.roleCache.TryGetValue(accessType, out var cached))
            {
                return cached;
            }

            Role? role = null;
            var mohClient = MohClients.FromAccessType(accessType);
            if (mohClient != null)
            {
                role = await this.client.GetClientRole(mohClient.Value.ClientId, mohClient.Value.AccessRole);
            }

            this.roleCache.Add(accessType, role);
            return role;
        }
    }
}
