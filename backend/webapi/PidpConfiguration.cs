namespace Pidp;

using Pidp.Infrastructure.Auth;

public class PidpConfiguration
{
    public static bool IsProduction() => EnvironmentName == Environments.Production;
    public static bool IsDevelopment() => EnvironmentName == Environments.Development;
    private static readonly string? EnvironmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

    public string ApplicationUrl { get; set; } = string.Empty;

    public AddressAutocompleteClientConfiguration AddressAutocompleteClient { get; set; } = new();
    public BCProviderClientConfiguration BCProviderClient { get; set; } = new();
    public ConnectionStringConfiguration ConnectionStrings { get; set; } = new();
    public ChesClientConfiguration ChesClient { get; set; } = new();
    public KeycloakConfiguration Keycloak { get; set; } = new();
    public LdapClientConfiguration LdapClient { get; set; } = new();
    public MailServerConfiguration MailServer { get; set; } = new();
    public PlrClientConfiguration PlrClient { get; set; } = new();
    public RabbitMQConfiguration RabbitMQ { get; set; } = new();

    // ------- Configuration Objects -------

    public class AddressAutocompleteClientConfiguration
    {
        public string ApiKey { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }

    public class BCProviderClientConfiguration
    {
        public string ClientId { get; set; } = string.Empty;
        public string TenantId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public string Domain { get; set; } = string.Empty;
    }

    public class ConnectionStringConfiguration
    {
        public string PidpDatabase { get; set; } = string.Empty;
    }

    public class ChesClientConfiguration
    {
        public bool Enabled { get; set; }
        public string Url { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public string TokenUrl { get; set; } = string.Empty;
    }

    public class KeycloakConfiguration
    {
        public string RealmUrl { get; set; } = string.Empty;
        public string WellKnownConfig => KeycloakUrls.WellKnownConfig(this.RealmUrl);
        public string TokenUrl => KeycloakUrls.Token(this.RealmUrl);
        public string AdministrationUrl { get; set; } = string.Empty;
        public string AdministrationClientId { get; set; } = string.Empty;
        public string AdministrationClientSecret { get; set; } = string.Empty;
        public string HcimClientId { get; set; } = string.Empty;
    }

    public class LdapClientConfiguration
    {
        public string Url { get; set; } = string.Empty;
    }

    public class MailServerConfiguration
    {
        public string Url { get; set; } = string.Empty;
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
    }

    public class PlrClientConfiguration
    {
        public string Url { get; set; } = string.Empty;
    }

    public class RabbitMQConfiguration
    {
        public string HostAddress { get; set; } = string.Empty;
    }
}
