namespace PlrTestData;

using Microsoft.EntityFrameworkCore;

using PlrIntake.Models;
using PlrIntake.Data;
using System.Globalization;
using Microsoft.Extensions.Configuration;

public class Program
{
    public static void Main()
    {
        var config = InitializeConfiguration();

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        var context = new PlrDbContext(new DbContextOptionsBuilder<PlrDbContext>()
            .UseNpgsql(config.ConnectionStrings.PlrDatabase)
            .Options);
        context.ChangeTracker.AutoDetectChangesEnabled = false;

        foreach (var card in config.TestCards)
        {
            foreach (var collegeIdentifier in config.PlrCollegeIdentifiers)
            {
                foreach (var status in PlrRecordStatus.Options)
                {
                    context.PlrRecords.Add(CreateRecordFrom(card, collegeIdentifier, status));
                }
            }
        }

        context.SaveChanges();
    }

    public static TestDataConfiguration InitializeConfiguration()
    {
        var configRoot = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var config = new TestDataConfiguration();
        configRoot.Bind(config);

        return config;
    }

    public static PlrRecord CreateRecordFrom(TestDataConfiguration.CardData card, string collegeIdentifier, (string Label, string StatusCode, string StatusReason) plrStatus)
    {
        // Good standing => 00015
        // Bad standing  => 90015
        var collegeIdPrefix = plrStatus.Label == "GS" ? "0" : "9";
        var collgeId = collegeIdPrefix + card.Id.ToString("D4", CultureInfo.InvariantCulture);
        var paddedCardNumber = card.Id.ToString("D5", CultureInfo.InvariantCulture);

        return new PlrRecord
        {
            Ipc = $"IPC.PIDP{paddedCardNumber}.{collegeIdentifier}.{plrStatus.Label}",
            Cpn = $"CPN.PIDP{paddedCardNumber}",
            IdentifierType = collegeIdentifier,
            CollegeId = collgeId,
            FirstName = card.FirstName,
            LastName = card.LastName,
            DateOfBirth = DateTime.ParseExact(card.Birthdate, "yyyy-MM-dd", CultureInfo.InvariantCulture),
            StatusCode = plrStatus.StatusCode,
            StatusReasonCode = plrStatus.StatusReason
        };
    }
}
