namespace Pidp.Infrastructure.HttpClients;

using Azure.Identity;
using IdentityModel.Client;
using Microsoft.Graph;

using Pidp.Extensions;
using Pidp.Infrastructure.HttpClients.AddressAutocomplete;
using Pidp.Infrastructure.HttpClients.BCProvider;
using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Infrastructure.HttpClients.Ldap;
using Pidp.Infrastructure.HttpClients.Mail;
using Pidp.Infrastructure.HttpClients.Plr;

public static class HttpClientSetup
{
    public static IServiceCollection AddHttpClients(this IServiceCollection services, PidpConfiguration config)
    {
        services.AddScoped<GraphServiceClient, GraphServiceClient>(services =>
        {
            // This configuration will need to change if our Authorization context changes, i.e. if we use any delegated access
            var options = new TokenCredentialOptions
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
            };
            var credential = new ClientSecretCredential(config.BCProviderClient.TenantId, config.BCProviderClient.ClientId, config.BCProviderClient.ClientSecret, options);

            // TODO: GraphServiceClient creates a new HttpClient using GraphClientFactory that has custom headers and middleware.
            // We should be injecting a managed HttpClient rather than creating a new one every time (to avoid socket exhaustion) but then it won't have that custom configuration.
            // Review this if/when Microsoft.Graph gives guidance on the propper pattern for doing this.
            return new GraphServiceClient(credential, ["https://graph.microsoft.com/.default"]);
        });

        services.AddHttpClient<IAccessTokenClient, AccessTokenClient>();

        services.AddHttpClientWithBaseAddress<IAddressAutocompleteClient, AddressAutocompleteClient>(config.AddressAutocompleteClient.Url);

        services.AddScoped<IBCProviderClient, BCProviderClient>();

        services.AddHttpClientWithBaseAddress<IChesClient, ChesClient>(config.ChesClient.Url)
            .WithBearerToken(new ChesClientCredentials
            {
                Address = config.ChesClient.TokenUrl,
                ClientId = config.ChesClient.ClientId,
                ClientSecret = config.ChesClient.ClientSecret
            });

        services.AddHttpClientWithBaseAddress<ILdapClient, LdapClient>(config.LdapClient.Url);

        services.AddHttpClientWithBaseAddress<IKeycloakAdministrationClient, KeycloakAdministrationClient>(config.Keycloak.AdministrationUrl)
            .WithBearerToken(new KeycloakAdministrationClientCredentials
            {
                Address = config.Keycloak.TokenUrl,
                ClientId = config.Keycloak.AdministrationClientId,
                ClientSecret = config.Keycloak.AdministrationClientSecret
            });

        services.AddHttpClientWithBaseAddress<IPlrClient, PlrClient>(config.PlrClient.Url);

        services.AddTransient<ISmtpEmailClient, SmtpEmailClient>();

        return services;
    }

    public static IHttpClientBuilder AddHttpClientWithBaseAddress<TClient, TImplementation>(this IServiceCollection services, string baseAddress)
        where TClient : class
        where TImplementation : class, TClient
        => services.AddHttpClient<TClient, TImplementation>(client => client.BaseAddress = new Uri(baseAddress.EnsureTrailingSlash()));

    public static IHttpClientBuilder WithBearerToken<T>(this IHttpClientBuilder builder, T credentials) where T : ClientCredentialsTokenRequest
    {
        builder.Services.AddSingleton(credentials)
            .AddTransient<BearerTokenHandler<T>>();

        builder.AddHttpMessageHandler<BearerTokenHandler<T>>();

        return builder;
    }
}
