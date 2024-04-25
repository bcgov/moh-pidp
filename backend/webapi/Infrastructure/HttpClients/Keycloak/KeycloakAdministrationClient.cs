namespace Pidp.Infrastructure.HttpClients.Keycloak;

using System.Net;

// TODO Use DomainResult for success/fail?
public class KeycloakAdministrationClient : BaseClient, IKeycloakAdministrationClient
{
    public KeycloakAdministrationClient(HttpClient httpClient, ILogger<KeycloakAdministrationClient> logger) : base(httpClient, logger) { }

    public async Task<bool> AssignAccessRoles(Guid userId, MohKeycloakEnrolment enrolment)
    {
        if (!enrolment.AccessRoles.Any())
        {
            return true;
        }

        // We need both the name and ID of the role to assign it.
        var roles = await this.GetClientRoles(enrolment.ClientId);
        if (roles == null)
        {
            return false;
        }

        var rolesToAssign = roles.IntersectBy(enrolment.AccessRoles, role => role.Name);
        if (rolesToAssign.Count() < enrolment.AccessRoles.Count())
        {
            // Some Roles were not found
            return false;
        }

        var result = await this.PostAsync($"users/{userId}/role-mappings/clients/{rolesToAssign.First().ContainerId}", rolesToAssign);
        if (result.IsSuccess)
        {
            this.Logger.LogClientRolesAssigned(userId, enrolment.AccessRoles, enrolment.ClientId);
        }

        return result.IsSuccess;
    }

    public async Task<bool> AssignClientRole(Guid userId, string clientId, string roleName)
    {
        // We need both the name and ID of the role to assign it.
        var role = await this.GetClientRole(clientId, roleName);
        if (role == null)
        {
            return false;
        }

        // Keycloak expects an array of roles.
        var result = await this.PostAsync($"users/{userId}/role-mappings/clients/{role.ContainerId}", new[] { role });
        if (result.IsSuccess)
        {
            this.Logger.LogClientRolesAssigned(userId, new[] { roleName }, clientId);
        }

        return result.IsSuccess;
    }

    public async Task<bool> AssignRealmRole(Guid userId, string roleName)
    {
        // We need both the name and ID of the role to assign it.
        var role = await this.GetRealmRole(roleName);
        if (role == null)
        {
            return false;
        }

        // Keycloak expects an array of roles.
        var result = await this.PostAsync($"users/{userId}/role-mappings/realm", new[] { role });
        if (result.IsSuccess)
        {
            this.Logger.LogRealmRoleAssigned(userId, roleName);
        }

        return result.IsSuccess;
    }

    public async Task<Guid?> CreateUser(UserRepresentation userRep)
    {
        if (userRep.Username == null)
        {
            return null;
        }

        var (status, location) = await this.PostWithLocationAsync("users", userRep);
        if (!status.IsSuccess)
        {
            this.Logger.LogUserCreationError(userRep);
            return null;
        }

        if (location != null)
        {
            if (Guid.TryParse(location.Segments.Last(), out var userId))
            {
                return userId;
            }
        }

        this.Logger.LogUserCreationLocationError(userRep.Username, location);

        var user = await this.FindUser(userRep.Username);
        if (Guid.TryParse(user?.Id, out var result))
        {
            return result;
        }

        this.Logger.LogCreatedUserNotFound();
        return null;
    }

    public async Task<UserRepresentation?> FindUser(string username)
    {
        var result = await this.GetWithQueryParamsAsync<List<UserRepresentation>>("users", new { username });
        if (!result.IsSuccess)
        {
            this.Logger.LogFindUserError(username);
            return null;
        }

        // Username query is a wildcard search, so would find both Bill and Maybill
        var filtered = result.Value.Where(user => string.Equals(user.Username, username, StringComparison.OrdinalIgnoreCase));
        if (filtered.Count() > 1)
        {
            this.Logger.LogFindMultipleUsersError(username);
            return null;
        }

        return filtered.SingleOrDefault();
    }

    public async Task<Client?> GetClient(string clientId)
    {
        var result = await this.GetAsync<IEnumerable<Client>>("clients");

        if (!result.IsSuccess)
        {
            return null;
        }

        var client = result.Value.SingleOrDefault(c => c.ClientId == clientId);

        if (client == null)
        {
            this.Logger.LogClientNotFound(clientId);
        }

        return client;
    }

    public async Task<Role?> GetClientRole(string clientId, string roleName)
    {
        var roles = await this.GetClientRoles(clientId);
        if (roles == null)
        {
            return null;
        }

        var role = roles.SingleOrDefault(r => r.Name == roleName);

        if (role == null)
        {
            this.Logger.LogClientRoleNotFound(roleName, clientId);
        }

        return role;
    }

    public async Task<IEnumerable<Role>?> GetClientRoles(string clientId)
    {
        // Need ID of Client (not the same as ClientId!) to fetch roles.
        var client = await this.GetClient(clientId);
        if (client == null)
        {
            return null;
        }

        var result = await this.GetAsync<IEnumerable<Role>>($"clients/{client.Id}/roles");

        if (!result.IsSuccess)
        {
            return null;
        }

        return result.Value;
    }

    public async Task<Role?> GetRealmRole(string roleName)
    {
        var result = await this.GetAsync<Role>($"roles/{WebUtility.UrlEncode(roleName)}");

        if (!result.IsSuccess)
        {
            return null;
        }

        return result.Value;
    }

    public async Task<UserRepresentation?> GetUser(Guid userId)
    {
        var result = await this.GetAsync<UserRepresentation>($"users/{userId}");
        if (!result.IsSuccess)
        {
            return null;
        }

        return result.Value;
    }

    public async Task<bool> RemoveClientRole(Guid userId, Role role)
    {
        if (role.ClientRole != true)
        {
            return false;
        }

        // Keycloak expects an array of roles.
        var response = await this.DeleteAsync($"users/{userId}/role-mappings/clients/{role.ContainerId}", new[] { role });

        return response.IsSuccess;
    }

    public async Task<bool> UpdateUser(Guid userId, UserRepresentation userRep)
    {
        var result = await this.PutAsync($"users/{userId}", userRep);
        return result.IsSuccess;
    }

    public async Task<bool> UpdateUser(Guid userId, Action<UserRepresentation> updateAction)
    {
        var user = await this.GetUser(userId);
        if (user == null)
        {
            return false;
        }

        updateAction(user);

        return await this.UpdateUser(userId, user);
    }
}

public static partial class KeycloakAdministrationClientLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Error, "Could not find a Client with ClientId {clientId} in Keycloak response.")]
    public static partial void LogClientNotFound(this ILogger<BaseClient> logger, string clientId);

    [LoggerMessage(2, LogLevel.Error, "Could not find a Client Role with name {roleName} from Client {clientId} in Keycloak response.")]
    public static partial void LogClientRoleNotFound(this ILogger<BaseClient> logger, string roleName, string clientId);

    [LoggerMessage(3, LogLevel.Information, "User {userId} was assigned Role(s) {roleNames} in Client {clientId}.")]
    public static partial void LogClientRolesAssigned(this ILogger<BaseClient> logger, Guid userId, IEnumerable<string> roleNames, string clientId);

    [LoggerMessage(4, LogLevel.Information, "User {userId} was assigned Realm Role {roleName}.")]
    public static partial void LogRealmRoleAssigned(this ILogger<BaseClient> logger, Guid userId, string roleName);

    [LoggerMessage(5, LogLevel.Error, "Error when creating a User with the representation: {userRep}.")]
    public static partial void LogUserCreationError(this ILogger<BaseClient> logger, UserRepresentation userRep);

    [LoggerMessage(6, LogLevel.Error, "Error when creating a User. Keycloak returned a success code but had a missing/malformed Location header. Username: {username}, Location header: {locationHeader}.")]
    public static partial void LogUserCreationLocationError(this ILogger<BaseClient> logger, string username, Uri? locationHeader);

    [LoggerMessage(7, LogLevel.Error, "Keycloak returned a success code but the user could not be found by either Location header or by searching for Username.")]
    public static partial void LogCreatedUserNotFound(this ILogger<BaseClient> logger);

    [LoggerMessage(8, LogLevel.Error, "Error when finding user with username {username}.")]
    public static partial void LogFindUserError(this ILogger<BaseClient> logger, string username);

    [LoggerMessage(9, LogLevel.Error, "Error when finding user with username {username}: multiple matching usernames found.")]
    public static partial void LogFindMultipleUsersError(this ILogger<BaseClient> logger, string username);
}
