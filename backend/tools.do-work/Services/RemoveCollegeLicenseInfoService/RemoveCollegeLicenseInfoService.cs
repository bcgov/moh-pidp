namespace DoWork.Services.RemoveCollegeLicenseInfoService;

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Pidp.Data;
using Pidp.Infrastructure.HttpClients.Keycloak;

public class RemoveCollegeLicenseInfoService(
    PidpDbContext context,
    IKeycloakAdministrationClient keycloakClient,
    ILogger<RemoveCollegeLicenseInfoService> logger) : IRemoveCollegeLicenseInfoService
{
    private readonly PidpDbContext context = context;
    private readonly IKeycloakAdministrationClient keycloakClient = keycloakClient;
    private readonly ILogger<RemoveCollegeLicenseInfoService> logger = logger;

    public async Task ExecuteAsync()
    {
        Console.WriteLine("Querying database for all Keycloak User IDs...");
        
        var userIds = await this.context.Credentials
            .Select(c => c.UserId)
            .Distinct()
            .ToListAsync();

        Console.WriteLine($"Found {userIds.Count} users to check in Keycloak.");

        var keysToRemove = new[] { "college_license_info", "college_licence_info", "college_certification_info" };
        var usersCleaned = 0;
        var usersNotFound = 0;
        var usersFailed = 0;

        for (var i = 0; i < userIds.Count; i++)
        {
            var userId = userIds[i];

            if (i % 100 == 0 && i > 0)
            {
                Console.WriteLine($"Checked {i} / {userIds.Count} users...");
            }

            var user = await this.keycloakClient.GetUser(userId);
            if (user == null)
            {
                usersNotFound++;
                continue;
            }

            if (user.Attributes == null)
            {
                continue;
            }

            var requiresUpdate = false;
            foreach (var key in keysToRemove)
            {
                if (user.Attributes.ContainsKey(key))
                {
                    user.Attributes.Remove(key);
                    requiresUpdate = true;
                    Console.WriteLine($"Found and removing '{key}' for User ID {userId}");
                }
            }

            if (requiresUpdate)
            {
                var success = await this.keycloakClient.UpdateUser(userId, user);
                if (success)
                {
                    usersCleaned++;
                }
                else
                {
                    usersFailed++;
                    Console.WriteLine($"Failed to update User ID {userId}");
                }
            }
        }

        Console.WriteLine("\n--- Migration Complete ---");
        Console.WriteLine($"Total users checked: {userIds.Count}");
        Console.WriteLine($"Users cleaned: {usersCleaned}");
        Console.WriteLine($"Users failed to update: {usersFailed}");
        Console.WriteLine($"Users not found in Keycloak: {usersNotFound}");
    }
}
