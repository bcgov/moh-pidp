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
        private readonly Dictionary<AccessTypeCode, Role?> roleCache;
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
            foreach (var access in party.AccessRequests)
            {
                var role = await this.FetchClientRole(access.AccessTypeCode);
                if (role == null)
                {
                    continue;
                }

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

                if (role == null)
                {
                    this.logger.LogClientRoleNotFound(accessType, mohClient.Value.AccessRole, mohClient.Value.ClientId);
                }
            }

            this.roleCache.Add(accessType, role);
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

    [LoggerMessage(3, LogLevel.Error, "For Access Type {accessType}, could not find a Client Role with name {roleName} in Client {clientId}.")]
    public static partial void LogClientRoleNotFound(this ILogger logger, AccessTypeCode accessType, string roleName, string clientId);
}
