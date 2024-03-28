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
        var userIds = await this.context.Parties
            .Where(party => party.Credentials.Any(credential => credential.IdentityProvider == IdentityProviders.BCServicesCard))
            .SelectMany(party => party.Credentials.Select(credential => credential.UserId))
            .ToListAsync();

        foreach (var userId in userIds)
        {
            try
            {
                if (null == await this.keycloakClient.GetUser(userId))
                {
                    Console.WriteLine($"User with ID {userId} not found in Keycloak.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error finding user with ID {userId}: {ex.Message}");
            }
        }

        Console.WriteLine("<<<<End!");
    }
}
