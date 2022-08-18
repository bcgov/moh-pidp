namespace Pidp.Features.Admin;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

using Pidp.Data;
using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Models.Lookups;

public class PartyDelete
{
    public class Command : ICommand { }

    public class CommandHandler : ICommandHandler<Command>
    {
        private readonly IMapper mapper;
        private readonly IKeycloakAdministrationClient client;
        private readonly PidpDbContext context;

        public CommandHandler(
            IMapper mapper,
            IKeycloakAdministrationClient client,
            PidpDbContext context)
        {
            this.mapper = mapper;
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

            // var roleRemover = await RoleRemover.CreateNew();


            this.context.Parties.RemoveRange(parties);
            await this.context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// Remember Keycloak Role representations between users to reduce total number of calls to keycloak.
    /// </summary>
    private class RoleRemover
    {
        // private readonly Dictionary<AccessTypeCode, Role> roleMap;

        // private RoleRemover(Dictionary<AccessTypeCode, Role> roleMap) => this.roleMap = roleMap;

        // public static async Task<RoleRemover> CreateNew()
        // {
        //     Dictionary<AccessTypeCode
        // }

        // public async Task<bool> RemoveClientRoles(PartyDelete party)
        // {

        // }
    }
}
