namespace Pidp.Infrastructure.HttpClients.BCProvider;

public class UserRepresentation
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Hpdid { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public string FullName => $"{this.FirstName} {this.LastName}";
}

/// <summary>
/// An AD Schema Extension to store additional attributes
/// </summary>
public class BCProviderSchemaExtension
{
    /// <summary>
    /// Level Of Assurance. Is 3 for a BC Provider created from a BC Services Card.
    /// </summary>
    public int? Loa { get; set; }
    public string? Hpdid { get; set; }

    public BCProviderSchemaExtension(int? loa, string? hpdid)
    {
        this.Loa = loa;
        this.Hpdid = hpdid;
    }

    public Dictionary<string, object> AsAdditionalData(string schemaExtensionId) => new() { { schemaExtensionId, this } };
}
