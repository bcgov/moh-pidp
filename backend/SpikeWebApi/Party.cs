using System.Text.Json;

namespace SpikeWebApi;

public class SpikeParty : IDisposable
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public JsonDocument? PlrRecord { get; set; }

    public void Dispose() => PlrRecord?.Dispose();
}

// public class SpikePlrRecord
// {
//     public string Cpn { get; set; } = string.Empty;
//     public string? Ipc { get; set; }
//     public string? FirstName { get; set; }
//     public string? LastName { get; set; }
//     public string? CollegeId { get; set; }
//     public string? IdentifierType { get; set; }
//     public string? ProviderRoleType { get; set; }
//     public string? StatusCode { get; set; }
//     public DateTime? StatusStartDate { get; set; }
//     public DateTime? StatusExpiryDate { get; set; }
//     public string? StatusReasonCode { get; set; }
//     public string? MspId { get; set; }
// }