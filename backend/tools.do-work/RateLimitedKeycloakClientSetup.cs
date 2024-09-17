namespace DoWork;

using System.IdentityModel.Tokens.Jwt;
using System.Threading.RateLimiting;
using IdentityModel.Client;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;

using Pidp;
using Pidp.Extensions;
using Pidp.Infrastructure.HttpClients;
using Pidp.Infrastructure.HttpClients.Keycloak;

public static class KeycloakClientSetup
{
    public static IServiceCollection AddRateLimitedKeycloakClient(this IServiceCollection services, PidpConfiguration config)
    {
        services.AddSingleton(new KeycloakAdministrationClientCredentials
        {
            Address = config.Keycloak.TokenUrl,
            ClientId = config.Keycloak.AdministrationClientId,
            ClientSecret = config.Keycloak.AdministrationClientSecret
        })
            .AddTransient<RateLimitedHandler>()
            .AddTransient<CachedBearerTokenHandler<KeycloakAdministrationClientCredentials>>();

        services.AddHttpClient<IAccessTokenClient, AccessTokenClient>();

        services.AddHttpClient<IKeycloakAdministrationClient, KeycloakAdministrationClient>()
            .AddHttpMessageHandler<RateLimitedHandler>()
            .AddHttpMessageHandler<CachedBearerTokenHandler<KeycloakAdministrationClientCredentials>>()
            .ConfigureHttpClient(client => client.BaseAddress = new Uri(config.Keycloak.AdministrationUrl.EnsureTrailingSlash()));

        return services;
    }
}

public class CachedBearerTokenHandler<T>(
    T credentials,
    IAccessTokenClient accessTokenClient,
    IClock clock) : DelegatingHandler where T : ClientCredentialsTokenRequest
{
    private readonly ClientCredentialsTokenRequest credentials = credentials ?? throw new ArgumentNullException(nameof(credentials));
    private readonly IAccessTokenClient tokenClient = accessTokenClient ?? throw new ArgumentNullException(nameof(accessTokenClient));
    private readonly IClock clock = clock ?? throw new ArgumentNullException(nameof(clock));

    private string accessToken = string.Empty;
    private Instant accessTokenExpiry = Instant.MinValue;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (this.clock.GetCurrentInstant() >= this.accessTokenExpiry)
        {
            this.accessToken = await this.tokenClient.GetAccessTokenAsync(this.credentials);
            this.accessTokenExpiry = Instant.FromDateTimeUtc(new JwtSecurityTokenHandler().ReadJwtToken(this.accessToken).Payload.ValidTo);
        }

        request.SetBearerToken(this.accessToken);

        return await base.SendAsync(request, cancellationToken);
    }
}

public class RateLimitedHandler() : DelegatingHandler
{
    // The Keycloak team has requested a rate limit of 5 requests per second (1 request per 200ms).
    private readonly FixedWindowRateLimiter limiter = new(new FixedWindowRateLimiterOptions { Window = TimeSpan.FromMilliseconds(200), PermitLimit = 1, QueueLimit = 1 });

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        using var lease = await this.limiter.AcquireAsync(cancellationToken: cancellationToken);

        return await base.SendAsync(request, cancellationToken);
    }
}
