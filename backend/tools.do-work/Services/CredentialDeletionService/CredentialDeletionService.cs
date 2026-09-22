namespace DoWork.Services.CredentialDeletionService;

using System.Globalization;
using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using NodaTime;

using Pidp.Data;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.BCProvider;
using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Models;

public class CredentialDeletionService(
    IBCProviderClient bcProviderClient,
    IClock clock,
    IKeycloakAdministrationClient keycloakClient,
    PidpDbContext context) : ICredentialDeletionService
{
    private readonly IBCProviderClient bcProviderClient = bcProviderClient;
    private readonly IClock clock = clock;
    private readonly IKeycloakAdministrationClient keycloakClient = keycloakClient;
    private readonly PidpDbContext context = context;

    public async Task DeleteCredentialsAsync(string? targetEmail = null)
    {
        var credentials = string.IsNullOrWhiteSpace(targetEmail) 
            ? ReadCredentialsFromFileAsync() 
            : new[] { targetEmail.Trim() };

        if (!credentials.Any())
        {
            Console.WriteLine("ERROR: No Credentials found.");
            return;
        }

        var foundCredentials = await this.context.Credentials
            .Where(credential => credentials.Any(x => EF.Functions.ILike(credential.IdpId!, x)))
            .OrderBy(credential => credential.IdpId)
            .ToListAsync();

        if (string.IsNullOrWhiteSpace(targetEmail))
        {
            Console.WriteLine($"{credentials.Count()} Credentials read from file.");
            Console.WriteLine($"{foundCredentials.Count} Credentials found in database.");
            if (credentials.Count() != foundCredentials.Count)
            {
                Console.WriteLine("ERROR: Number of credentials found in CredentialsToDelete does not match number of credentials found in Database.");
                return;
            }

            this.WriteCredentialsToFileAsync(foundCredentials);
            Console.WriteLine("Details of the Credentials found in the Database have been saved. Re-enter the count to delete them from the database.");
        }
        else
        {
            Console.WriteLine($"{foundCredentials.Count} Credentials found in database for email: {targetEmail}");
            foreach (var credential in foundCredentials)
            {
                Console.WriteLine($"{credential.IdpId}\t{credential.UserId}");
            }

            if (foundCredentials.Count == 0)
            {
                Console.WriteLine("No credentials found in Database. Aborting.");
                return;
            }

            Console.WriteLine("Details of the Credentials found in the Database have been output to the screen. Re-enter the count to delete them from the database.");
        }

        if (int.TryParse(Console.ReadLine(), out var count) && count == foundCredentials.Count)
        {
            // foreach (var credential in foundCredentials)
            // {
            //     if (credential.IdentityProvider == IdentityProviders.BCProvider && !string.IsNullOrEmpty(credential.IdpId))
            //     {
            //         await this.bcProviderClient.DeleteBCProviderAccount(credential.IdpId);
            //     }
            //     if (credential.UserId != Guid.Empty)
            //     {
            //         await this.keycloakClient.DeleteUser(credential.UserId);
            //     }
            // }

            this.context.Credentials.RemoveRange(foundCredentials);
            await this.context.SaveChangesAsync();
            // Console.WriteLine($"Credentials deleted from database, Keycloak, and BC Provider AD.");
            Console.WriteLine($"Credentials deleted from database.");
        }
        else
        {
            Console.WriteLine("Aborting. No Credentials deleted.");
        }
    }

    private static IEnumerable<string> ReadCredentialsFromFileAsync()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resources = assembly.GetManifestResourceNames()
            .Where(name => name.EndsWith("CredentialsToDelete", StringComparison.OrdinalIgnoreCase));
        
        Stream? stream = null;
        if (resources.Count() == 1)
        {
            stream = assembly.GetManifestResourceStream(resources.Single());
        }
        else
        {
            var outputDir = "output";
            var path1 = Path.Combine(outputDir, "CredentialsToDelete");
            var path2 = Path.Combine(outputDir, "CredentialsToDelete.txt");

            if (File.Exists(path1))
            {
                stream = File.OpenRead(path1);
            }
            else if (File.Exists(path2))
            {
                stream = File.OpenRead(path2);
            }
            else
            {
                throw new InvalidOperationException($"Could not find the CredentialsToDelete file as an embedded resource or in {outputDir} directory.");
            }
        }

        using var reader = new StreamReader(stream ?? throw new InvalidOperationException("Could not open stream to read CredentialsToDelete file."));
        while (true)
        {
            var line = reader.ReadLine()?.Trim();
            if (string.IsNullOrWhiteSpace(line))
            {
                yield break;
            }
            if (line.StartsWith("//", StringComparison.OrdinalIgnoreCase))
            {
                // Skip comments
                continue;
            }
            yield return line;
        }
    }

    private void WriteCredentialsToFileAsync(IEnumerable<Credential> credentials)
    {
        var fileName = this.clock.GetCurrentInstant().ToString("yyyyMMdd_HH-mm-ss", CultureInfo.InvariantCulture) + "Credentials.txt";

        using var stream = File.Create(fileName);
        using var writer = new StreamWriter(stream);
        foreach (var credential in credentials)
        {
            writer.WriteLine($"{credential.IdpId}\t{credential.UserId}");
        }

        writer.Flush();
    }
}
