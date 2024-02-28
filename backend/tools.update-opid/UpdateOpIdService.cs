namespace UpdateOpId;

using Microsoft.EntityFrameworkCore;

using Pidp.Data;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.Keycloak;

public class UpdateOpIdService : IUpdateOpIdService
{
    private readonly PidpDbContext context;
    private readonly IKeycloakAdministrationClient keycloakClient;

    public UpdateOpIdService(PidpDbContext context, IKeycloakAdministrationClient keycloakClient)
    {
        this.context = context;
        this.keycloakClient = keycloakClient;
    }

    public async Task UpdateOpIdAsync()
    {
        var parties = await this.context.Parties
            .Include(party => party.Credentials)
            .Take(1000)
            .Where(party => party.OpId == null
                && party.Credentials.Any(credential => credential.IdentityProvider == IdentityProviders.BCServicesCard))
            .ToListAsync();

        foreach (var party in parties)
        {
            await party.GenerateOpId(this.context);

            foreach (var userId in party.Credentials.Select(credential => credential.UserId))
            {
                await this.keycloakClient.UpdateUser(userId, user => user.SetOpId(party.OpId!));
            }
        }

        await this.context.SaveChangesAsync();
    }
}
