namespace DoWork;

using Microsoft.EntityFrameworkCore;

using Pidp.Data;
using Pidp.Infrastructure.Auth;
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
