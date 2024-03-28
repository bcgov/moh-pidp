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
        Console.WriteLine(">>>>Start!");
        var parties = await this.context.Parties
            .Include(party => party.Credentials)
            .Where(party => party.Credentials.Any(credential => credential.IdentityProvider == IdentityProviders.BCServicesCard))
            .ToListAsync();

        foreach (var party in parties)
        {
            foreach (var userId in party.Credentials.Select(credential => credential.UserId))
            {
                try
                {
                    var user = await this.keycloakClient.GetUser(userId);
                    if (user == null)
                    {
                        Console.WriteLine($"User with ID {userId} not found in Keycloak.");
                        continue;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error finding user with ID {userId}: {ex.Message}");
                }
            }
        }

        Console.WriteLine("<<<<End!");

        await this.context.SaveChangesAsync();
    }
}
