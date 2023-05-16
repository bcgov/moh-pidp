namespace Pidp.Infrastructure.HttpClients.BCProvider;

using System.Text.Json;

public class UserRepresentation
{
    public string Cpn { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Hpdid { get; set; } = string.Empty;
    public bool IsMd { get; set; }
    public bool IsMoa { get; set; }
    public bool IsRnp { get; set; }
    public string Password { get; set; } = string.Empty;
    public string PidpEmail { get; set; } = string.Empty;

    public string FullName => $"{this.FirstName} {this.LastName}";
}

/// <summary>
/// An AD Directory Extension to store additional attributes
/// </summary>
public class BCProviderAttributes
{
    private readonly string extensionNamePrefix;
    private readonly Dictionary<string, object> attributes = new();

    public BCProviderAttributes(string clientId) => this.extensionNamePrefix = $"extension_{clientId.Replace("-", "")}_";

    public static BCProviderAttributes FromNewUser(string clientId, UserRepresentation representation)
    {
        return new BCProviderAttributes(clientId)
            .SetCpn(representation.Cpn)
            .SetHpdid(representation.Hpdid)
            .SetIsMd(representation.IsMd)
            .SetIsMoa(representation.IsMoa)
            .SetIsRnp(representation.IsRnp)
            .SetLoa(3)
            .SetPidpEmail(representation.PidpEmail);
    }

    public Dictionary<string, object> AsAdditionalData() => this.attributes;

    public BCProviderAttributes SetCpn(string cpn) => this.SetProperty(nameof(cpn), cpn);
    public BCProviderAttributes SetHpdid(string hpdid) => this.SetProperty(nameof(hpdid), hpdid);
    public BCProviderAttributes SetIsMd(bool isMd) => this.SetProperty(nameof(isMd), isMd);
    public BCProviderAttributes SetIsMoa(bool isMoa) => this.SetProperty(nameof(isMoa), isMoa);
    public BCProviderAttributes SetIsRnp(bool isRnp) => this.SetProperty(nameof(isRnp), isRnp);
    /// <summary>
    /// Level Of Assurance. Is 3 for a BC Provider created from a BC Services Card.
    /// </summary>
    public BCProviderAttributes SetLoa(int loa) => this.SetProperty(nameof(loa), loa);
    public BCProviderAttributes SetPidpEmail(string pidpEmail) => this.SetProperty(nameof(pidpEmail), pidpEmail);

    private BCProviderAttributes SetProperty(string propertyName, object value)
    {
        this.attributes[this.extensionNamePrefix + propertyName] = value;
        return this;
    }
}
