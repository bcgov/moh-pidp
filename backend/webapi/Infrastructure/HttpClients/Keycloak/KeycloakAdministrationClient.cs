namespace Pidp.Infrastructure.HttpClients.Keycloak;

using System.Net;

// TODO Use DomainResult for success/fail?
public class KeycloakAdministrationClient : BaseClient, IKeycloakAdministrationClient
{
    public KeycloakAdministrationClient(HttpClient httpClient, ILogger<KeycloakAdministrationClient> logger) : base(httpClient, logger) { }

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
            this.Logger.LogClientRoleAssigned(userId, roleName, clientId);
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
        var response = await this.PostAsync($"users/{userId}/role-mappings/realm", new[] { role });
        if (response.IsSuccess)
        {
            this.Logger.LogRealmRoleAssigned(userId, roleName);
        }

        return response.IsSuccess;
    }

    public async Task<Client?> GetClient(string clientId)
    {
        var result = await this.GetAsync<IEnumerable<Client>>("clients");

        if (!result.IsSuccess)
        {
            return null;
        }

        var client = result.Value?.SingleOrDefault(c => c.ClientId == clientId);

        if (client == null)
        {
            this.Logger.LogClientNotFound(clientId);
        }

        return client;
    }

    public async Task<Role?> GetClientRole(string clientId, string roleName)
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

        var role = result.Value?.SingleOrDefault(r => r.Name == roleName);

        if (role == null)
        {
            this.Logger.LogClientRoleNotFound(roleName, clientId);
        }

        return role;
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
    public static partial void LogClientNotFound(this ILogger logger, string clientId);

    [LoggerMessage(2, LogLevel.Error, "Could not find a Client Role with name {roleName} from Client {clientId} in Keycloak response.")]
    public static partial void LogClientRoleNotFound(this ILogger logger, string roleName, string clientId);

    [LoggerMessage(3, LogLevel.Information, "User {userId} was assigned Role {roleName} in Client {clientId}.")]
    public static partial void LogClientRoleAssigned(this ILogger logger, Guid userId, string roleName, string clientId);

    [LoggerMessage(4, LogLevel.Information, "User {userId} was assigned Realm Role {roleName}.")]
    public static partial void LogRealmRoleAssigned(this ILogger logger, Guid userId, string roleName);
}
