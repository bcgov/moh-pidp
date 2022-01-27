namespace Pidp;

using Pidp.Infrastructure.Auth;

public class PidpConfiguration
{
    public static bool IsDevelopment() => EnvironmentName == Environments.Development;
    private static readonly string? EnvironmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

    public AddressAutocompleteConfiguration AddressAutocompleteClient { get; set; } = new();
    public ConnectionStringConfiguration ConnectionStrings { get; set; } = new();
    public KeycloakConfiguration Keycloak { get; set; } = new();
    public PlrClientConfiguration PlrClient { get; set; } = new();

    // ------- Configuration Objects -------

    public class AddressAutocompleteConfiguration
    {
        public string ApiKey { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }

    public class ConnectionStringConfiguration
    {
        public string PidpDatabase { get; set; } = string.Empty;
    }

    public class KeycloakConfiguration
    {
        public string RealmUrl { get; set; } = string.Empty;
        public string WellKnownConfig => KeycloakUrls.WellKnownConfig(this.RealmUrl);
        public string TokenUrl => KeycloakUrls.Token(this.RealmUrl);
        public string AdministrationUrl { get; set; } = string.Empty;
        public string AdministrationClientId { get; set; } = string.Empty;
        public string AdministrationClientSecret { get; set; } = string.Empty;
    }

    public class PlrClientConfiguration
    {
        public string Url { get; set; } = string.Empty;
    }
}
