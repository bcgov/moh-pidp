namespace Pidp.Infrastructure.HttpClients.Plr;

using System.Threading.Tasks;

using Pidp.Models.Lookups;

public class PlrClient : BaseClient, IPlrClient
{
    public PlrClient(HttpClient client) : base(client, PropertySerialization.CamelCase) { }

    public async Task<PlrRecord> GetPlrRecord(string licenceNumber, CollegeCode collegeCode)
    {
        throw new NotImplementedException();
    }
}

public class PlrRecord
{
    /// <summary>
    /// Internal Party Code, PLRs unique identifier for each record.
    /// </summary>
    public string Ipc { get; set; } = string.Empty;
}
