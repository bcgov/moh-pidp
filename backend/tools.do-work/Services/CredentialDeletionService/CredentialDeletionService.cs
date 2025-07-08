namespace DoWork.Services.CredentialDeletionService;

using System.Globalization;
using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using NodaTime;

using Pidp.Data;
using Pidp.Models;

public class CredentialDeletionService(IClock clock, PidpDbContext context) : ICredentialDeletionService
{
    private readonly IClock clock = clock;
    private readonly PidpDbContext context = context;

    public async Task DeleteCredentialsAsync()
    {
        var credentials = ReadCredentialsFromFileAsync();
        if (!credentials.Any())
        {
            Console.WriteLine("No Credentials found in CredentialsToDelete file.");
            return;
        }

        var foundCredentials = await this.context.Credentials
            .Where(credential => credentials.Any(x => EF.Functions.ILike(credential.IdpId!, x)))
            .OrderBy(credential => credential.IdpId)
            .ToListAsync();

        Console.WriteLine($"{credentials.Count()} Credentials read from file.");
        Console.WriteLine($"{foundCredentials.Count} Credentials found in database.");
        if (credentials.Count() != foundCredentials.Count)
        {
            Console.WriteLine($">> Warning: number of credentials found in CredentialsToDelete does not match number of credentials found in Database.");
        }

        this.WriteCredentialsToFileAsync(foundCredentials);
        Console.WriteLine("Details of the Credentials found in the Database have been saved. Re-enter the count to delete them from the database.");

        if (int.TryParse(Console.ReadLine(), out var count) && count == foundCredentials.Count)
        {
            this.context.Credentials.RemoveRange(foundCredentials);
            await this.context.SaveChangesAsync();
            Console.WriteLine($"Credentials deleted from database.");
        }
        else
        {
            Console.WriteLine("No Credentials deleted.");
        }
    }

    private static IEnumerable<string> ReadCredentialsFromFileAsync()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resources = assembly.GetManifestResourceNames()
            .Where(name => name.EndsWith("CredentialsToDelete", StringComparison.OrdinalIgnoreCase));
        if (resources.Count() != 1)
        {
            throw new InvalidOperationException("Could not find the CredentialsToDelete file.");
        }

        using var stream = assembly.GetManifestResourceStream(resources.Single());
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
