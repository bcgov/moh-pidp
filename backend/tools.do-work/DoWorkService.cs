namespace DoWork;

using Microsoft.Extensions.DependencyInjection;

using DoWork.Services.BulkEformRoles;
using DoWork.Services.CredentialDeletionService;
using DoWork.Services.ResyncService;

/// <summary>
/// Modify this file with custom scripts / helper services.
/// Remember to check that any dependencies you need (like the Keycloak or BC Provider client) are registered in the Program.cs file and the nessisary environment variables have been added or modified in appsettings.json.
/// </summary>
public class DoWorkService(IServiceProvider services) : IDoWorkService
{
    private readonly IServiceProvider services = services;

    public async Task DoWorkAsync()
    {
        Console.WriteLine("Select a service to run:");
        Console.WriteLine("1. Credential Deletion");
        Console.WriteLine("2. Resync Service");
        Console.WriteLine("3. Access Role Cleanup - re-evaluate one card's holders and revoke those who no longer qualify");
        Console.WriteLine("4. Infant RSV Bulk Grant - back-grant Infant RSV to eligible Imms/SA holders");
        Console.Write("Enter your choice (1-4): ");

        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                await this.services.GetRequiredService<ICredentialDeletionService>().DeleteCredentialsAsync();
                break;
            case "2":
                Console.Write("Run in Dry-Run mode? (y/n, default y): ");
                var dryRunChoice = Console.ReadLine();
                var dryRun = dryRunChoice?.Trim().ToLower() != "n";
                await this.services.GetRequiredService<IResyncService>().SynchronizeAsync(dryRun);
                break;
            case "3":
                await this.services.GetRequiredService<IAccessRoleCleanupService>().CleanupAsync();
                break;
            case "4":
                await this.services.GetRequiredService<IInfantRsvBulkGrantService>().GrantAsync();
                break;
            default:
                Console.WriteLine("Invalid choice.");
                break;
        }
    }
}
