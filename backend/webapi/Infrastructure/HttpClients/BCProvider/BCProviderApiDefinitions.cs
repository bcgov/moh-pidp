namespace Pidp.Infrastructure.HttpClients.BCProvider;

using System.Reflection;

public class NewUserRepresentation
{
    public string? Cpn { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Hpdid { get; set; } = string.Empty;
    public bool IsMd { get; set; }
    public bool IsMoa { get; set; }
    public bool IsRnp { get; set; }
    public string PidpEmail { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string FullName => $"{this.FirstName} {this.LastName}";
}

public static class BCProviderAttributeName
{
    public const string Cpn = "cpn";
    public const string Hpdid = "hpdid";
    public const string IsMd = "isMd";
    public const string IsMoa = "isMoa";
    public const string IsRnp = "isRnp";
    public const string Loa = "loa";
    public const string PidpEmail = "pidpEmail";

    public static string[] GetValues()
    {
        return typeof(BCProviderAttributeName)
            .GetFields(BindingFlags.Static | BindingFlags.Public)
            .Where(f => f.FieldType == typeof(string))
            .Select(f => (string)f.GetValue(null)!)
            .ToArray();
    }
}

/// <summary>
/// An AD Directory Extension to store additional attributes
/// </summary>
public class BCProviderAttributes
{
    private readonly string extensionNamePrefix;
    private readonly Dictionary<string, object> attributes = new();

    public BCProviderAttributes(string clientId) => this.extensionNamePrefix = $"extension_{clientId.Replace("-", "")}_";

    public static BCProviderAttributes FromNewUser(string clientId, NewUserRepresentation representation)
    {
        var attributes = new BCProviderAttributes(clientId)
            .SetHpdid(representation.Hpdid)
            .SetIsMd(representation.IsMd)
            .SetIsMoa(representation.IsMoa)
            .SetIsRnp(representation.IsRnp)
            .SetLoa(3)
            .SetPidpEmail(representation.PidpEmail);

        if (!string.IsNullOrWhiteSpace(representation.Cpn))
        {
            attributes.SetCpn(representation.Cpn);
        }

        return attributes;
    }

    public string[] GetBCProviderAttributeKeys()
    {
        return BCProviderAttributeName
            .GetValues()
            .Select(attributeName => this.GetBCProviderAttributeKey(attributeName))
            .ToArray();
    }

    public string GetBCProviderAttributeKey(string attributeName) => this.extensionNamePrefix + attributeName;

    public Dictionary<string, object> AsAdditionalData() => this.attributes;

    public BCProviderAttributes SetCpn(string cpn) => this.SetProperty(BCProviderAttributeName.Cpn, cpn);
    public BCProviderAttributes SetHpdid(string hpdid) => this.SetProperty(BCProviderAttributeName.Hpdid, hpdid);
    public BCProviderAttributes SetIsMd(bool isMd) => this.SetProperty(BCProviderAttributeName.IsMd, isMd);
    public BCProviderAttributes SetIsMoa(bool isMoa) => this.SetProperty(BCProviderAttributeName.IsMoa, isMoa);
    public BCProviderAttributes SetIsRnp(bool isRnp) => this.SetProperty(BCProviderAttributeName.IsRnp, isRnp);
    /// <summary>
    /// Level Of Assurance. Is 3 for a BC Provider created from a BC Services Card.
    /// </summary>
    public BCProviderAttributes SetLoa(int loa) => this.SetProperty(BCProviderAttributeName.Loa, loa);
    public BCProviderAttributes SetPidpEmail(string pidpEmail) => this.SetProperty(BCProviderAttributeName.PidpEmail, pidpEmail);

    private BCProviderAttributes SetProperty(string propertyName, object value)
    {
        this.attributes[this.extensionNamePrefix + propertyName] = value;
        return this;
    }
}
