namespace DoWork;

/// <summary>
/// Modify this file with custom scripts / helper services.
/// Remember to check that any dependencies you need (like the Keycloak or BC Provider client) are registered in the Program.cs file and the nessisary environment variables have been added or modified in appsettings.json.
/// </summary>
public class DoWorkService(
    DoWork.Services.CredentialDeletionService.ICredentialDeletionService credentialDeletionService,
    DoWork.Services.ResyncService.IResyncService resyncService) : IDoWorkService
{
    private readonly DoWork.Services.CredentialDeletionService.ICredentialDeletionService credentialDeletionService = credentialDeletionService;
    private readonly DoWork.Services.ResyncService.IResyncService resyncService = resyncService;

    public async Task DoWorkAsync()
    {
        Console.WriteLine("Select a service to run:");
        Console.WriteLine("1. Credential Deletion");
        Console.WriteLine("2. Resync Service");
        Console.Write("Enter your choice (1-2): ");
        
        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                await this.credentialDeletionService.DeleteCredentialsAsync();
                break;
            case "2":
                Console.Write("Run in Dry-Run mode? (y/n, default y): ");
                var dryRunChoice = Console.ReadLine();
                var dryRun = dryRunChoice?.Trim().ToLower() != "n";
                await this.resyncService.SynchronizeAsync(dryRun);
                break;
            default:
                Console.WriteLine("Invalid choice.");
                break;
        }
    }
}
