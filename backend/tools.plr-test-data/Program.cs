namespace PlrTestData;

using System.Globalization;
using Microsoft.EntityFrameworkCore;

using PlrIntake.Data;
using PlrIntake.Models;
using PlrTestData.Data;

public class Program
{
    public static void Main(string[] args)
    {
        var connectionString = args?.FirstOrDefault() ?? "Host=localhost;Port=5433;Database=postgres;Username=postgres;Password=postgres";

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        var context = new PlrDbContext(new DbContextOptionsBuilder<PlrDbContext>()
            .UseNpgsql(connectionString)
            .Options);
        context.ChangeTracker.AutoDetectChangesEnabled = false;

        foreach (var card in TestData.Cards)
        {
            foreach (var collegeIdentifier in TestData.PlrCollegeIdentifiers)
            {
                foreach (var status in TestData.PlrRecordStatuses)
                {
                    context.PlrRecords.Add(CreateRecordFrom(card, collegeIdentifier, status));
                }
            }
        }

        context.SaveChanges();
    }

    public static PlrRecord CreateRecordFrom(TestCard card, string collegeIdentifier, PlrRecordStatus plrStatus)
    {
        var fourDigitCardId = card.Id.ToString("D4", CultureInfo.InvariantCulture);

        return new PlrRecord
        {
            Ipc = $"IPC.PIDP0{fourDigitCardId}.{collegeIdentifier}.{plrStatus.StatusName}",
            Cpn = $"CPN.PIDP0{fourDigitCardId}.{collegeIdentifier}.{plrStatus.StatusName}",
            IdentifierType = collegeIdentifier,
            CollegeId = plrStatus.CollegeIdPrefix + fourDigitCardId,
            FirstName = card.FirstName,
            LastName = card.LastName,
            DateOfBirth = DateTime.ParseExact(card.Birthdate, "yyyy-MM-dd", CultureInfo.InvariantCulture),
            StatusCode = plrStatus.StatusCode,
            StatusReasonCode = plrStatus.StatusReasonCode
        };
    }
}
