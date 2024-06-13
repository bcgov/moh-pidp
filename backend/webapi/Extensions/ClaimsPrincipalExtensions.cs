namespace Pidp.Extensions;

using NodaTime;
using NodaTime.Text;
using System.Security.Claims;
using System.Text.Json;

using Pidp.Infrastructure.Auth;

public static class ClaimsPrincipalExtensions
{
    /// <summary>
    /// Returns the UserId of the logged in user (from the 'sub' claim). If there is no logged in user, this will return Guid.Empty
    /// </summary>
    public static Guid GetUserId(this ClaimsPrincipal? user)
    {
        var userId = user?.FindFirstValue(Claims.Subject);

        return Guid.TryParse(userId, out var parsed)
            ? parsed
            : Guid.Empty;
    }

    /// <summary>
    /// Returns the Birthdate Claim of the User, parsed in ISO format (yyyy-MM-dd)
    /// </summary>
    public static LocalDate? GetBirthdate(this ClaimsPrincipal user)
    {
        var birthdate = user.FindFirstValue(Claims.Birthdate);

        var parsed = LocalDatePattern.Iso.Parse(birthdate!);
        if (parsed.Success)
        {
            return parsed.Value;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Returns the Identity Provider of the User, or null if User is null
    /// </summary>
    public static string? GetIdentityProvider(this ClaimsPrincipal? user) => user?.FindFirstValue(Claims.IdentityProvider);

    /// <summary>
    /// Returns the Identity Provider ID of the User, or null if User is null.
    /// Trims "@bcp" off the end if the Identity Provider is BC Provider.
    /// </summary>
    public static string? GetIdpId(this ClaimsPrincipal? user)
    {
        var idpId = user?.FindFirstValue(Claims.PreferredUsername);

        if (idpId != null
            && user.GetIdentityProvider() == IdentityProviders.BCProvider
            && idpId.EndsWith("@bcp", StringComparison.InvariantCultureIgnoreCase))
        {
            // Keycloak adds "@<identity provider>" at the end of the IDP ID, and for BC Providers this won't match what we have in the DB if we don't trim it.
            idpId = idpId[..^4];
        }

        return idpId;
    }

    /// <summary>
    /// Parses the Resource Access claim and returns the roles for the given resource
    /// </summary>
    /// <param name="resourceName">The name of the resource to retrive the roles from</param>
    public static IEnumerable<string> GetResourceAccessRoles(this ClaimsIdentity identity, string resourceName)
    {
        var resourceAccessClaim = identity.Claims
            .SingleOrDefault(claim => claim.Type == Claims.ResourceAccess)
            ?.Value;

        if (string.IsNullOrWhiteSpace(resourceAccessClaim))
        {
            return [];
        }

        try
        {
            var resources = JsonSerializer.Deserialize<Dictionary<string, ResourceAccess>>(resourceAccessClaim, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            return resources?.TryGetValue(resourceName, out var access) == true
                ? access.Roles
                : [];
        }
        catch
        {
            return [];
        }
    }

    private sealed class ResourceAccess
    {
        public IEnumerable<string> Roles { get; set; } = [];
    }
}
