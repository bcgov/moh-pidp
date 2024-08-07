namespace DoWork;

using Microsoft.EntityFrameworkCore;

using Pidp.Data;
using Pidp.Infrastructure.HttpClients.Keycloak;

public class DoWorkService(IKeycloakAdministrationClient keycloakClient, PidpDbContext context) : IDoWorkService
{
    private readonly IKeycloakAdministrationClient keycloakClient = keycloakClient;
    private readonly PidpDbContext context = context;

    public async Task DoWorkAsync()
    {
        var userId = await this.context.Parties
            .Where(party => party.Id == 1001)
            .Select(party => party.Credentials.First().UserId)
            .SingleOrDefaultAsync();
        await this.keycloakClient.GetUser(userId);
    }
}
