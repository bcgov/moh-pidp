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
            .Where(party => party.Credentials.Any(credential => credential.IdentityProvider == IdentityProviders.BCServicesCard))
            .ToListAsync();

        foreach (var party in parties)
        {
            if (party.OpId != null)
            {
                continue;
            }

            party.GenerateOpId(this.context);

            foreach (var userId in party.Credentials.Select(credential => credential.UserId))
            {
                if (party.OpId != null)
                {
                    await this.keycloakClient.UpdateUserOpId(userId, party.OpId);
                }
            }
        }

        await this.context.SaveChangesAsync();
    }
}
