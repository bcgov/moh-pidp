namespace Pidp.Infrastructure.HttpClients.Plr;

using Flurl;
using NodaTime;
using System.Threading.Tasks;

using Pidp.Models.Lookups;

public class PlrClient : BaseClient, IPlrClient
{
    public PlrClient(HttpClient client, ILogger<PlrClient> logger) : base(client, logger) { }

    public async Task<string?> GetPlrRecord(string licenceNumber, CollegeCode collegeCode, LocalDate birthdate)
    {
        throw new NotImplementedException();
        var url = "records".SetQueryParams(new
        {
            CollegeId = licenceNumber,
            Birthdate = birthdate.ToString()
        });

        var response = await this.Client.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            this.Logger.LogError($"Error when retrieving PLR Record with CollegeId = {licenceNumber} and Birthdate = {birthdate}.");
            return null;
        }

        var records = await response.Content.ReadFromJsonAsync<IEnumerable<PlrRecord>>();

    }

    private class PlrRecord
    {
        public string Ipc { get; set; } = string.Empty;
        public string? IdentifierType { get; set; }
    }
}
