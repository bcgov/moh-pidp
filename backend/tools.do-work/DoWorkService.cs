namespace DoWork;

using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Pidp;
using Pidp.Data;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.BCProvider;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models.Lookups;

public class PartyDto
{
    public string? Cpn { get; set; }
    public string? Upn { get; set; }
}

public class DoWorkService(
    IBCProviderClient bcProviderClient,
    PidpDbContext context,
    IPlrClient plrClient,
    PidpConfiguration config) : IDoWorkService
{
    private readonly IBCProviderClient bcProviderClient = bcProviderClient;
    private readonly PidpDbContext context = context;
    private readonly IPlrClient plrClient = plrClient;
    private readonly string clientId = config.BCProviderClient.ClientId;


    public async Task DoWorkAsync()
    {
        var partyDtos = await this.context.Parties
            .Where(party => party.Cpn != null
                && party.Credentials.Any(credential => credential.IdentityProvider == IdentityProviders.BCProvider)
                && party.LicenceDeclaration.CollegeCode == CollegeCode.Pharmacists)
            .Select(party => new
            {
                party.Cpn,
                Upn = party.Credentials
                    .Where(credential => credential.IdentityProvider == IdentityProviders.BCProvider)
                    .Select(credential => credential.IdpId)
                    .FirstOrDefault()
            })
            .ToListAsync();

        Console.WriteLine($">>>>parties loaded! Count: {partyDtos.Count}");

        var batchSize = 1000;
        var fileIndex = 1;
        List<PartyDto> batch = [];

        foreach (var item in partyDtos)
        {
            batch.Add(new PartyDto { Cpn = item.Cpn, Upn = item.Upn });

            if (batch.Count == batchSize)
            {
                await WriteBatchToFileAsync(batch, fileIndex);
                fileIndex++;
                batch.Clear();
            }
        }

        // Write remaining cpns if any
        if (batch.Count > 0)
        {
            await WriteBatchToFileAsync(batch, fileIndex);
        }

        var userDtos = await ReadCpnsFromFileAsync("users_batch_1.json");

        Console.WriteLine($">>>>User data loaded from file! Count: {userDtos.Count}");

        // Process the cpns as needed
        foreach (var user in userDtos)
        {
            var plrStanding = await this.plrClient.GetStandingsDigestAsync(user.Cpn);

            var attributes = new BCProviderAttributes(this.clientId)
                .SetIsPharm(plrStanding.With(IdentifierType.Pharmacist).HasGoodStanding);

            await this.bcProviderClient.UpdateAttributes(user.Upn, attributes.AsAdditionalData());
        }
    }

    private static async Task<List<PartyDto>> ReadCpnsFromFileAsync(string fileName)
    {
        if (!File.Exists(fileName))
        {
            throw new FileNotFoundException($"The file {fileName} does not exist.");
        }

        var json = await File.ReadAllTextAsync(fileName);
        var result = JsonSerializer.Deserialize<List<PartyDto>>(json);
        return result ?? [];
    }

    private static async Task WriteBatchToFileAsync(List<PartyDto> batch, int fileIndex)
    {
        var fileName = $"users_batch_{fileIndex}.json";
        var json = JsonSerializer.Serialize(batch, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(fileName, json);
        Console.WriteLine($">>>>Batch {fileIndex} written to {fileName}");
    }
}

