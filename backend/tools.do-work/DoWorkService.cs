namespace DoWork;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using DoWork.Services.BulkEformRoles;
using DoWork.Services.CredentialDeletionService;
using DoWork.Services.ResyncService;

/// <summary>
/// Modify this file with custom scripts / helper services.
/// Remember to check that any dependencies you need (like the Keycloak or BC Provider client) are registered in the Program.cs file and the nessisary environment variables have been added or modified in appsettings.json.
/// </summary>
public class DoWorkService(IServiceProvider services, IConfiguration configuration) : IDoWorkService
{
    private readonly IServiceProvider services = services;
    private readonly IConfiguration configuration = configuration;

    public async Task DoWorkAsync()
    {
        var serviceArg = this.configuration["service"] ?? this.configuration["Service"];

        if (serviceArg?.Equals("credential-deletion", StringComparison.OrdinalIgnoreCase) == true)
        {
            var emailArg = this.configuration["email"] ?? this.configuration["Email"];
            Console.WriteLine("Running CredentialDeletionService");
            await this.services.GetRequiredService<ICredentialDeletionService>().DeleteCredentialsAsync(emailArg);
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

            await this.services.GetRequiredService<IResyncService>().SynchronizeAsync(dryRunParsed, parsedPartyId);
            return;
        }
        else if (serviceArg?.Equals("access-role-cleanup", StringComparison.OrdinalIgnoreCase) == true)
        {
            // Dry run unless --apply --from <report> was passed; BulkRunOptions owns those flags and
            // Program.cs has already validated them before the host was built.
            await this.services.GetRequiredService<IAccessRoleCleanupService>().CleanupAsync();
            return;
        }
        else if (serviceArg?.Equals("infant-rsv-bulk-grant", StringComparison.OrdinalIgnoreCase) == true)
        {
            await this.services.GetRequiredService<IInfantRsvBulkGrantService>().GrantAsync();
            return;
        }

        var buildTimestamp = System.IO.File.GetLastWriteTime(System.Reflection.Assembly.GetExecutingAssembly().Location);
        Console.WriteLine($"Usage: dotnet run -- --service=<service-name> [options]");
        Console.WriteLine($"Last Updated: {buildTimestamp:yyyy-MM-dd HH:mm:ss}");
        Console.WriteLine();
        Console.WriteLine("Available Services:");
        Console.WriteLine("  --service=resync                 Runs the ResyncService.");
        Console.WriteLine("                                   Options:");
        Console.WriteLine("                                     --partyid=<id>    (Optional) Sync a specific party by ID. If omitted, syncs all parties.");
        Console.WriteLine("                                     --dryrun=false    (Optional) Disable dry-run mode to apply changes. Defaults to true.");
        Console.WriteLine();
        Console.WriteLine("  --service=credential-deletion    Runs the CredentialDeletionService.");
        Console.WriteLine("                                   Options:");
        Console.WriteLine("                                     --email=<email>   (Optional) Delete credentials for a specific BC Provider email. Outputs to screen instead of file.");
        Console.WriteLine();
        Console.WriteLine("  --service=access-role-cleanup    Re-evaluates one card's holders and revokes those who no longer qualify.");
        Console.WriteLine("                                   Options:");
        Console.WriteLine("                                     --access-type=<name>  (Optional) Limit to one Access Type, e.g. ImmsBCEforms.");
        Console.WriteLine("                                     --apply --from=<csv>  (Optional) Act on the Parties in a reviewed report. Dry run without it.");
        Console.WriteLine();
        Console.WriteLine("  --service=infant-rsv-bulk-grant  Back-grants Infant RSV to eligible Imms/SA holders.");
        Console.WriteLine("                                   Options:");
        Console.WriteLine("                                     --apply --from=<csv>  (Optional) Act on the Parties in a reviewed report. Dry run without it.");
        Console.WriteLine();
    }
}
