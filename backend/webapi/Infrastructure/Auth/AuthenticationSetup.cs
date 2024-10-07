namespace Pidp.Infrastructure.Auth;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

using Pidp.Extensions;

public static class AuthenticationSetup
{
    public static IServiceCollection AddKeycloakAuth(this IServiceCollection services, PidpConfiguration config)
    {
        services.ThrowIfNull(nameof(services));
        config.ThrowIfNull(nameof(config));

        JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();

        services.AddAuthentication()
        .AddJwtBearer(options =>
        {
            options.Authority = config.Keycloak.RealmUrl;
            options.Audience = Clients.PidpApi;
            options.MetadataAddress = config.Keycloak.WellKnownConfig;
            options.Events = new JwtBearerEvents
            {
                OnTokenValidated = async context => await OnTokenValidatedAsync(context)
            };
        });

        var anyPartyIdentityProviderPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .RequireClaim(Claims.IdentityProvider, IdentityProviders.BCServicesCard, IdentityProviders.BCProvider, IdentityProviders.Idir, IdentityProviders.Phsa)
            .Build();

        services.AddAuthorizationBuilder()
            .AddPolicy(Policies.BcscAuthentication, policy => policy
                .RequireAuthenticatedUser()
                .RequireClaim(Claims.IdentityProvider, IdentityProviders.BCServicesCard))
            .AddPolicy(Policies.BCProviderAuthentication, policy => policy
                .RequireAuthenticatedUser()
                .RequireClaim(Claims.IdentityProvider, IdentityProviders.BCProvider))
            .AddPolicy(Policies.HighAssuranceIdentityProvider, policy => policy
                .RequireAuthenticatedUser()
                .RequireClaim(Claims.IdentityProvider, IdentityProviders.BCServicesCard, IdentityProviders.BCProvider))
            .AddPolicy(Policies.IdirAuthentication, policy => policy
                .RequireAuthenticatedUser()
                .RequireClaim(Claims.IdentityProvider, IdentityProviders.Idir))
            .AddPolicy(Policies.AnyPartyIdentityProvider, anyPartyIdentityProviderPolicy)
            .SetFallbackPolicy(anyPartyIdentityProviderPolicy);

        return services;
    }

    private static Task OnTokenValidatedAsync(TokenValidatedContext context)
    {
        if (context.Principal?.Identity is ClaimsIdentity identity
            && identity.IsAuthenticated)
        {
            // Flatten the Resource Access claim
            identity.AddClaims(identity.GetResourceAccessRoles(Clients.PidpApi)
                .Select(role => new Claim(ClaimTypes.Role, role)));
        }

        return Task.CompletedTask;
    }
}
