namespace Pidp.Extensions;

using NodaTime;
using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;

using Pidp.Infrastructure.Auth;
using NodaTime.Text;

public static class ClaimsPrincipalExtensions
{
    /// <summary>
    /// Returns the Guid of the logged in user. If there is no logged in user, this will return Guid.Empty
    /// </summary>
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var userId = user?.FindFirstValue(Claims.Subject);

        return Guid.TryParse(userId, out var parsed)
            ? parsed
            : Guid.Empty;
    }

    public static int GetIdentityAssuranceLevel(this ClaimsPrincipal user)
    {
        var assuranceLevel = user?.FindFirstValue(Claims.AssuranceLevel);

        return int.TryParse(assuranceLevel, out var parsed)
            ? parsed
            : 0;
    }

    /// <summary>
    /// Returns the Birthdate of the User, parsed in ISO format (yyyy-MM-dd)
    /// </summary>
    public static LocalDate? GetDateOfBirth(this ClaimsPrincipal user)
    {
        var birthdate = user.FindFirstValue(Claims.Birthdate);

        var parsed = LocalDatePattern.Iso.Parse(birthdate);
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
    /// Parses the Resourse Access claim and returns the roles for the given resource
    /// </summary>
    /// <param name="resourceName">The name of the resource to retrive the roles from</param>
    public static IEnumerable<string> GetResourceAccessRoles(this ClaimsIdentity identity, string resourceName)
    {
        var resourceAccessClaim = identity.Claims
            .SingleOrDefault(claim => claim.Type == Claims.ResourceAccess)
            ?.Value;

        if (string.IsNullOrWhiteSpace(resourceAccessClaim))
        {
            return Enumerable.Empty<string>();
        }

        var resources = JsonSerializer.Deserialize<Dictionary<string, ResourceAccess>>(resourceAccessClaim, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })!;
        return resources.TryGetValue(resourceName, out var access)
            ? access.Roles
            : Enumerable.Empty<string>();
    }

    private class ResourceAccess
    {
        public IEnumerable<string> Roles { get; set; } = Enumerable.Empty<string>();
    }
}
