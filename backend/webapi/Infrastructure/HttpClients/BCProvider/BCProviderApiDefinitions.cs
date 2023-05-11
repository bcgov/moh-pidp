namespace Pidp.Infrastructure.HttpClients.BCProvider;

public class UserRepresentation
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Hpdid { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string PidpEmail { get; set; } = string.Empty;

    public string FullName => $"{this.FirstName} {this.LastName}";
}

/// <summary>
/// An AD Directory Extension to store additional attributes
/// </summary>
public class BCProviderDirectoryExtension
{
    /// <summary>
    /// Level Of Assurance. Is 3 for a BC Provider created from a BC Services Card.
    /// </summary>
    public int? Loa { get; set; }
    public string? Hpdid { get; set; }
    public string? PidpEmail { get; set; }
    public bool? IsMd { get; set; }
    public bool? IsRnp { get; set; }

    public BCProviderDirectoryExtension(int? loa, string? hpdid, string? pidpEmail, bool? isMd, bool? isRnp)
    {
        this.Loa = loa;
        this.Hpdid = hpdid;
        this.PidpEmail = pidpEmail;
        this.IsMd = isMd;
        this.IsRnp = isRnp;
    }

    public Dictionary<string, object> AsAdditionalData(string schemaExtensionId) => new() { { schemaExtensionId, this } };
}
