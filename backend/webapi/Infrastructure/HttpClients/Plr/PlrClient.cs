namespace Pidp.Infrastructure.HttpClients.Plr;

using NodaTime;
using System.Threading.Tasks;

using Pidp.Models.Lookups;
using Pidp.Extensions;

public class PlrClient : BaseClient, IPlrClient
{
    public PlrClient(HttpClient client, ILogger<PlrClient> logger) : base(client, logger) { }

    public async Task<string?> GetPlrRecord(CollegeCode collegeCode, string licenceNumber, LocalDate birthdate)
    {
        // TODO timeout of 4-5 seconds
        var response = await this.Client.GetWithQueryParamsAsync("records", new { CollegeId = licenceNumber, Birthdate = birthdate.ToString() });
        if (!response.IsSuccessStatusCode)
        {
            this.Logger.LogError($"Error when retrieving PLR Record with CollegeId = {licenceNumber} and Birthdate = {birthdate}.");
            return null;
        }

        var records = await response.Content.ReadFromJsonAsync<IEnumerable<PlrRecord>>();
        if (records == null || !records.Any())
        {
            this.Logger.LogInformation($"No Records found in PLR with CollegeId = {licenceNumber} and Birthdate = {birthdate}.");
            return null;
        }

        records = records
            .Where(record => record.MapIdentifierTypeToCollegeCode() == collegeCode);
        if (records.Count() > 1)
        {
            this.Logger.LogInformation($"Multiple matching Records found in PLR with CollegeId = {licenceNumber}, Birthdate = {birthdate}, and IdentifierType matching CollegeCode {collegeCode}.");
            return null;
        }

        return records.Single().Ipc;
    }

    private class PlrRecord
    {
        public string Ipc { get; set; } = string.Empty;
        public string? IdentifierType { get; set; }

        public CollegeCode? MapIdentifierTypeToCollegeCode()
        {
            return this.IdentifierType switch
            {
                "PHID" or "PHTID " => CollegeCode.Pharmacists,
                "CPSID" => CollegeCode.PhysiciansAndSurgeons,
                "RNID" or "RMID" => CollegeCode.NursesAndMidwives,
                _ => null
            };
        }
    }
}
