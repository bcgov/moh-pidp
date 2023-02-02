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
        private readonly ILogger logger;
        private readonly PidpDbContext context;

        public CommandHandler(
            IKeycloakAdministrationClient client,
            ILogger<CommandHandler> logger,
            PidpDbContext context)
        {
            this.client = client;
            this.logger = logger;
            this.context = context;
        }

        public async Task HandleAsync(Command command)
        {
            var parties = await this.context.Parties
                .Include(party => party.AccessRequests)
                .ToListAsync();

            if (!parties.Any())
            {
                return;
            }

            var roleRemover = new RoleRemover(this.client, this.logger);
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
        private readonly Dictionary<string, Role?> roleCache;
        private readonly IKeycloakAdministrationClient client;
        private readonly ILogger logger;

        public RoleRemover(IKeycloakAdministrationClient client, ILogger logger)
        {
            this.client = client;
            this.logger = logger;
            this.roleCache = new();
        }

        public async Task RemoveClientRoles(Party party)
        {
            foreach (var role in await this.DetermineRoles(party.AccessRequests))
            {
                if (await this.client.RemoveClientRole(party.UserId, role))
                {
                    this.logger.LogRemoveSuccess(role.Name!, party.UserId);
                }
                else
                {
                    this.logger.LogRemoveFailure(role.Name!, party.UserId);
                }
            }
        }

        private async Task<IEnumerable<Role>> DetermineRoles(IEnumerable<AccessRequest> accessRequests)
        {
            var roleList = new List<Role?>();
            foreach (var accessRequest in accessRequests)
            {
                var clientInfo = MohClients.FromAccessType(accessRequest.AccessTypeCode);
                if (clientInfo != null)
                {
                    roleList.Add(await this.GetOrAddRole(clientInfo.Value.ClientId, clientInfo.Value.AccessRole));
                }
            }

            roleList.Add(await this.GetOrAddRole(MohClients.LicenceStatus.ClientId, MohClients.LicenceStatus.MoaRole));
            roleList.Add(await this.GetOrAddRole(MohClients.LicenceStatus.ClientId, MohClients.LicenceStatus.PractitionerRole));

            return roleList.Where(role => role != null).Cast<Role>();
        }

        private async Task<Role?> GetOrAddRole(string clientId, string roleName)
        {
            if (this.roleCache.TryGetValue(roleName, out var cached))
            {
                return cached;
            }

            var role = await this.client.GetClientRole(clientId, roleName);
            if (role == null)
            {
                this.logger.LogClientRoleNotFound(roleName, clientId);
            }

            this.roleCache.Add(roleName, role);
            return role;
        }
    }
}

public static partial class PartyDeleteLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Error, "Removed {roleName} from {userId}.")]
    public static partial void LogRemoveSuccess(this ILogger logger, string roleName, Guid userId);

    [LoggerMessage(2, LogLevel.Error, "Could not remove {roleName} from {userId}.")]
    public static partial void LogRemoveFailure(this ILogger logger, string roleName, Guid userId);

    [LoggerMessage(3, LogLevel.Error, "Could not find a Client Role with name {roleName} in Client {clientId}.")]
    public static partial void LogClientRoleNotFound(this ILogger logger, string roleName, string clientId);
}
