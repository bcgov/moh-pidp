namespace Pidp.Infrastructure.HttpClients.BCProvider;

using System.Text.Json;

public class UserRepresentation
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Hpdid { get; set; } = string.Empty;
    public bool IsMd { get; set; }
    public bool IsRnp { get; set; }
    public string Password { get; set; } = string.Empty;
    public string PidpEmail { get; set; } = string.Empty;

    public string FullName => $"{this.FirstName} {this.LastName}";
}

/// <summary>
/// An AD Directory Extension to store additional attributes
/// </summary>
public class BCProviderDirectoryExtension
{
    public string? Hpdid { get; set; }
    /// <summary>
    /// Level Of Assurance. Is 3 for a BC Provider created from a BC Services Card.
    /// </summary>
    public int? Loa { get; set; }
    public string? PidpEmail { get; set; }
    public bool? IsMd { get; set; }
    public bool? IsRnp { get; set; }

    public BCProviderDirectoryExtension() { }

    public BCProviderDirectoryExtension(UserRepresentation representation)
    {
        this.Hpdid = representation.Hpdid;
        this.Loa = 3;
        this.PidpEmail = representation.PidpEmail;
        this.IsMd = representation.IsMd;
        this.IsRnp = representation.IsRnp;
    }

    public Dictionary<string, object?> AsAdditionalData(string clientId)
    {
        var keyPrefix = $"extension_{clientId}_";

        return this.GetType().GetProperties()
            .ToDictionary(prop => keyPrefix + JsonNamingPolicy.CamelCase.ConvertName(prop.Name), prop => prop.GetValue(this));
    }
}
