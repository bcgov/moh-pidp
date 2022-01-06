namespace Pidp.Auth;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Pidp.Extensions;

public static class AuthenticationSetup
{
    public static IServiceCollection InitializeAuth(this IServiceCollection services, PidpConfiguration config)
    {
        services.ThrowIfNull(nameof(services));
        config.ThrowIfNull(nameof(config));

        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        services.AddAuthentication()
        .AddJwtBearer(options =>
        {
            options.Authority = config.Keycloak.RealmUrl;
            options.Audience = AuthConstants.Audience;
            options.MetadataAddress = config.Keycloak.WellKnownConfig;
            options.Events = new JwtBearerEvents
            {
                OnTokenValidated = async context => await OnTokenValidatedAsync(context)
            };
        });

        // services.AddAuthorization(options => );

        return services;
    }

    private static Task OnTokenValidatedAsync(TokenValidatedContext context)
    {
        if (context.SecurityToken is JwtSecurityToken accessToken
                && context.Principal?.Identity is ClaimsIdentity identity
                && identity.IsAuthenticated)
        {
            identity.AddClaim(new Claim(ClaimTypes.Name, accessToken.Subject));

            // FlattenRealmAccessRoles(identity);
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Flattens the Realm Access claim, as Microsoft Identity Model doesn't support nested claims
    /// </summary>
    // private static void FlattenRealmAccessRoles(ClaimsIdentity identity)
    // {
    //     var realmAccessClaim = identity.Claims
    //         .SingleOrDefault(claim => claim.Type == Claims.RealmAccess)
    //         ?.Value;

    //     if (realmAccessClaim != null)
    //     {
    //         var realmAccess = JsonConvert.DeserializeObject<RealmAccess>(realmAccessClaim);

    //         identity.AddClaims(realmAccess.Roles.Select(role => new Claim(ClaimTypes.Role, role)));
    //     }
    // }

    // private class RealmAccess
    // {
    //     public string[] Roles { get; set; }
    // }
}
