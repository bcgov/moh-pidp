using Microsoft.Extensions.Configuration;

namespace DoWork;

/// <summary>
/// Modify this file with custom scripts / helper services.
/// Remember to check that any dependencies you need (like the Keycloak or BC Provider client) are registered in the Program.cs file and the nessisary environment variables have been added or modified in appsettings.json.
/// </summary>
public class DoWorkService(
    DoWork.Services.CredentialDeletionService.ICredentialDeletionService credentialDeletionService,
    DoWork.Services.ResyncService.IResyncService resyncService,
    IConfiguration configuration) : IDoWorkService
{
    private readonly DoWork.Services.CredentialDeletionService.ICredentialDeletionService credentialDeletionService = credentialDeletionService;
    private readonly DoWork.Services.ResyncService.IResyncService resyncService = resyncService;
    private readonly IConfiguration configuration = configuration;

    public async Task DoWorkAsync()
    {
        var serviceArg = this.configuration["service"] ?? this.configuration["Service"];

        if (serviceArg?.Equals("credential-deletion", StringComparison.OrdinalIgnoreCase) == true)
        {
            Console.WriteLine("Running CredentialDeletionService");
            await this.credentialDeletionService.DeleteCredentialsAsync();
            return;
        }
        else if (serviceArg?.Equals("resync", StringComparison.OrdinalIgnoreCase) == true)
        {
            var partyIdArg = this.configuration["partyid"] ?? this.configuration["partyId"] ?? this.configuration["PartyId"];
            int? parsedPartyId = null;
            if (!string.IsNullOrWhiteSpace(partyIdArg) && int.TryParse(partyIdArg, out var id))
            {
                parsedPartyId = id;
            }

            var dryRunArg = this.configuration["dryrun"] ?? this.configuration["dryRun"] ?? this.configuration["DryRun"];
            var dryRunParsed = dryRunArg?.Trim().ToLower() != "false";

            Console.WriteLine($"Running ResyncService for PartyId {parsedPartyId?.ToString() ?? "ALL"} (DryRun: {dryRunParsed})");

            // Give the user time to abort
            Thread.Sleep(5000);

            await this.resyncService.SynchronizeAsync(dryRunParsed, parsedPartyId);
            return;
        }

        Console.WriteLine("Usage: dotnet run -- --service=<service-name> [options]");
        Console.WriteLine();
        Console.WriteLine("Available Services:");
        Console.WriteLine("  --service=resync                 Runs the ResyncService.");
        Console.WriteLine("                                   Options:");
        Console.WriteLine("                                     --partyid=<id>    (Optional) Sync a specific party by ID. If omitted, syncs all parties.");
        Console.WriteLine("                                     --dryrun=false    (Optional) Disable dry-run mode to apply changes. Defaults to true.");
        Console.WriteLine();
        Console.WriteLine("  --service=credential-deletion    Runs the CredentialDeletionService.");
        Console.WriteLine();
    }
}
