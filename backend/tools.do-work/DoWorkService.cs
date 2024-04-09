namespace DoWork;

using Microsoft.EntityFrameworkCore;

using Pidp.Data;
using Pidp.Infrastructure.HttpClients.Keycloak;

public class DoWorkService : IDoWorkService
{
    private readonly IKeycloakAdministrationClient keycloakClient;
    private readonly PidpDbContext context;

    public DoWorkService(IKeycloakAdministrationClient keycloakClient, PidpDbContext context)
    {
        this.keycloakClient = keycloakClient;
        this.context = context;
    }

    public async Task DoWorkAsync()
    {
        var userId = await this.context.Parties
            .Where(party => party.Id == 1001)
            .Select(party => party.Credentials.First().UserId)
            .SingleOrDefaultAsync();
        await this.keycloakClient.GetUser(userId);
    }
}
