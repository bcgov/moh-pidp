namespace Pidp.Infrastructure.HttpClients;

using IdentityModel.Client;

using Pidp.Extensions;
using Pidp.Infrastructure.HttpClients.AddressAutocomplete;
using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Infrastructure.HttpClients.Ldap;
using Pidp.Infrastructure.HttpClients.Mail;
using Pidp.Infrastructure.HttpClients.Plr;

public static class HttpClientSetup
{
    public static IServiceCollection AddHttpClients(this IServiceCollection services, PidpConfiguration config)
    {
        services.AddHttpClient<IAccessTokenClient, AccessTokenClient>();

        services.AddHttpClientWithBaseAddress<IAddressAutocompleteClient, AddressAutocompleteClient>(config.AddressAutocompleteClient.Url);

        services.AddHttpClientWithBaseAddress<IChesClient, ChesClient>(config.ChesClient.Url)
            .WithBearerToken(new ChesClientCredentials
            {
                Address = config.ChesClient.TokenUrl,
                ClientId = config.ChesClient.ClientId,
                ClientSecret = config.ChesClient.ClientSecret
            });

        services.AddHttpClientWithBaseAddress<ILdapClient, LdapClient>("https://common-logon-dev.hlth.gov.bc.ca"); //TODO config

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
