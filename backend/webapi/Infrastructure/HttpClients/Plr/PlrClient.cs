namespace Pidp.Infrastructure.HttpClients.Plr;

using System.Threading.Tasks;

using Pidp.Models.Lookups;

public class PlrClient : BaseClient, IPlrClient
{
    public PlrClient(HttpClient client, ILogger<PlrClient> logger) : base(client, logger) { }

    public async Task<PlrRecord?> GetPlrRecord(string licenceNumber, CollegeCode collegeCode)
    {
        throw new NotImplementedException();
        var response = await this.Client.GetAsync("");
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var records = await response.Content.ReadFromJsonAsync<IEnumerable<PlrIntake.Features.Records.Index.Model>>();

    }
}

public class PlrRecord
{
    /// <summary>
    /// Internal Party Code, PLRs unique identifier for each record.
    /// </summary>
    public string Ipc { get; set; } = string.Empty;

    public string
}
