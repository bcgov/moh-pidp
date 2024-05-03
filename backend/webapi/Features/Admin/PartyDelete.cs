namespace Pidp.Features.Admin;

using Microsoft.EntityFrameworkCore;

using Pidp.Data;
using Pidp.Extensions;
using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Models;

public class PartyDelete
{
    public class Command : ICommand
    {
        public int? PartyId { get; set; }
    }

    public class CommandHandler : ICommandHandler<Command>
    {
        private readonly IKeycloakAdministrationClient client;
        private readonly ILogger<CommandHandler> logger;
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
                .Include(party => party.Credentials)
                .Include(party => party.AccessRequests)
                .If(command.PartyId.HasValue, q => q
                    .Where(party => party.Id == command.PartyId))
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
        private readonly Dictionary<MohKeycloakEnrolment, IEnumerable<Role>> roleCache;
        private readonly IKeycloakAdministrationClient client;
        private readonly ILogger<CommandHandler> logger;

        public RoleRemover(IKeycloakAdministrationClient client, ILogger<CommandHandler> logger)
        {
            this.client = client;
            this.logger = logger;
            this.roleCache = new();
        }

        public async Task RemoveClientRoles(Party party)
        {
            foreach (var role in await this.DetermineRoles(party))
            {
                foreach (var credential in party.Credentials)
                {
                    if (!await this.client.RemoveClientRole(credential.UserId, role))
                    {
                        this.logger.LogRemoveFailure(role.Name!, party.PrimaryUserId);
                    }
                }
            }
        }

        private async Task<IEnumerable<Role>> DetermineRoles(Party party)
        {
            var enrolments = party.AccessRequests
                .Select(accessRequest => MohKeycloakEnrolment.FromAssociatedAccessRequest(accessRequest.AccessTypeCode))
                .Where(enrolment => enrolment != null);

            if (string.IsNullOrWhiteSpace(party.Cpn))
            {
                enrolments = enrolments.Append(MohKeycloakEnrolment.MoaLicenceStatus);
            }
            else
            {
                enrolments = enrolments.Append(MohKeycloakEnrolment.PractitionerLicenceStatus);
            }

            List<Role> roles = new();
            foreach (var enrolment in enrolments)
            {
                roles.AddRange(await this.GetOrAddRoles(enrolment!));
            }
            return roles;
        }

        private async Task<IEnumerable<Role>> GetOrAddRoles(MohKeycloakEnrolment enrolment)
        {
            if (this.roleCache.TryGetValue(enrolment, out var cached))
            {
                return cached;
            }

            List<Role> roles = new();
            foreach (var roleName in enrolment.AccessRoles)
            {
                var role = await this.client.GetClientRole(enrolment.ClientId, roleName);
                if (role == null)
                {
                    this.logger.LogClientRoleNotFound(roleName, enrolment.ClientId);
                    throw new InvalidOperationException("Error Comunicating with Keycloak");
                }

                roles.Add(role);
            }

            this.roleCache.Add(enrolment, roles);
            return roles;
        }
    }
}

public static partial class PartyDeleteLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Error, "Could not remove {roleName} from {userId}.")]
    public static partial void LogRemoveFailure(this ILogger<PartyDelete.CommandHandler> logger, string roleName, Guid userId);

    [LoggerMessage(2, LogLevel.Error, "Could not find a Client Role with name {roleName} in Client {clientId}.")]
    public static partial void LogClientRoleNotFound(this ILogger<PartyDelete.CommandHandler> logger, string roleName, string clientId);
}
