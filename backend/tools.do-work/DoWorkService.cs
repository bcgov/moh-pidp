namespace DoWork;

/// <summary>
/// Modify this file with custom scripts / helper services.
/// Remember to check that any dependencies you need (like the Keycloak or BC Provider client) are registered in the Program.cs file and the nessisary environment variables have been added or modified in appsettings.json.
/// </summary>
public class DoWorkService(
    DoWork.Services.DataDriftFixService.IDataDriftFixService dataDriftFixService,
    DoWork.Services.KeycloakClientValidationService.IKeycloakClientValidationService keycloakClientValidationService,
    DoWork.Services.RemoveCollegeLicenseInfoService.IRemoveCollegeLicenseInfoService removeCollegeLicenseInfoService,
    DoWork.Services.CredentialDeletionService.ICredentialDeletionService credentialDeletionService) : IDoWorkService
{
    private readonly DoWork.Services.DataDriftFixService.IDataDriftFixService dataDriftFixService = dataDriftFixService;
    private readonly DoWork.Services.KeycloakClientValidationService.IKeycloakClientValidationService keycloakClientValidationService = keycloakClientValidationService;
    private readonly DoWork.Services.RemoveCollegeLicenseInfoService.IRemoveCollegeLicenseInfoService removeCollegeLicenseInfoService = removeCollegeLicenseInfoService;
    private readonly DoWork.Services.CredentialDeletionService.ICredentialDeletionService credentialDeletionService = credentialDeletionService;

    public async Task DoWorkAsync()
    {
        Console.WriteLine("Select a service to run:");
        Console.WriteLine("1. Data Drift Fix");
        Console.WriteLine("2. Keycloak Client Validation");
        Console.WriteLine("3. Remove College License Info");
        Console.WriteLine("4. Credential Deletion");
        Console.Write("Enter your choice (1-4): ");
        
        var choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                await this.dataDriftFixService.FixDataDriftAsync();
                break;
            case "2":
                await this.keycloakClientValidationService.ValidateClientsAsync();
                break;
            case "3":
                await this.removeCollegeLicenseInfoService.ExecuteAsync();
                break;
            case "4":
                await this.credentialDeletionService.DeleteCredentialsAsync();
                break;
            default:
                Console.WriteLine("Invalid choice.");
                break;
        }
    }
}
