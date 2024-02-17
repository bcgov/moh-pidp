namespace UpdateOpId;

using Microsoft.EntityFrameworkCore;
using NanoidDotNet;

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
            //TODO Need to move business logic like generating OpID and guarantee uniqueness to the Party.cs model
            party.OpId = Nanoid.Generate("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ");

            foreach (var userId in party.Credentials.Select(credential => credential.UserId))
            {
                await this.keycloakClient.UpdateUserOpId(userId, party.OpId);
            }
        }

        await this.context.SaveChangesAsync();
    }
}
