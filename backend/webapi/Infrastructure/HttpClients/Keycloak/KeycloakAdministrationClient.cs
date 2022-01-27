namespace Pidp.Infrastructure.HttpClients.Keycloak;

using System.Net;

public class KeycloakAdministrationClient : BaseClient, IKeycloakAdministrationClient
{
    public KeycloakAdministrationClient(HttpClient httpClient, ILogger<KeycloakAdministrationClient> logger) : base(httpClient, logger) { }

    public async Task<Client?> GetClient(string clientId)
    {
        var response = await this.Client.GetAsync("clients");

        if (!response.IsSuccessStatusCode)
        {
            var responseMessage = await response.Content.ReadAsStringAsync();
            this.Logger.LogError($"Could not retrieve the Clients from Keycloak. Response message: {responseMessage}");
            return null;
        }

        var clients = await response.Content.ReadFromJsonAsync<IEnumerable<Client>>();
        var client = clients?.SingleOrDefault(c => c.ClientId == clientId);

        if (client == null)
        {
            this.Logger.LogError($"Could not find a Client with ClientId {clientId} from Keycloak.");
        }

        return client;
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
        var content = this.CreateStringContent(new[] { role });
        var response = await this.Client.PostAsync($"users/{userId}/role-mappings/clients/{role.ContainerId}", content);

        if (!response.IsSuccessStatusCode)
        {
            var responseMessage = await response.Content.ReadAsStringAsync();
            this.Logger.LogError($"Could not assign the role {role} to user {userId}. Response message: {responseMessage}");
            return false;
        }

        return true;
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
        var content = this.CreateStringContent(new[] { role });
        var response = await this.Client.PostAsync($"users/{userId}/role-mappings/realm", content);

        if (!response.IsSuccessStatusCode)
        {
            var responseMessage = await response.Content.ReadAsStringAsync();
            this.Logger.LogError($"Could not assign the role {roleName} to user {userId}. Response message: {responseMessage}");
            return false;
        }

        return true;
    }

    public async Task<Role?> GetClientRole(string clientId, string roleName)
    {
        // Need ID of Client (not the same as ClientId!) to fetch roles.
        var client = await this.GetClient(clientId);
        if (client == null)
        {
            return null;
        }

        var response = await this.Client.GetAsync($"clients/{client.Id}/roles");

        if (!response.IsSuccessStatusCode)
        {
            var responseMessage = await response.Content.ReadAsStringAsync();
            this.Logger.LogError($"Could not retrieve the Roles from Client {client.Id}. Response message: {responseMessage}");
            return null;
        }

        var roles = await response.Content.ReadFromJsonAsync<IEnumerable<Role>>();
        var role = roles?.SingleOrDefault(r => r.Name == roleName);

        if (role == null)
        {
            this.Logger.LogError($"Could not find a Client Role with name {roleName} from Client {clientId}.");
        }

        return role;
    }

    public async Task<Role?> GetRealmRole(string roleName)
    {
        var response = await this.Client.GetAsync($"roles/{WebUtility.UrlEncode(roleName)}");

        if (!response.IsSuccessStatusCode)
        {
            var responseMessage = await response.Content.ReadAsStringAsync();
            this.Logger.LogError($"Could not retrieve the role {roleName} from Keycloak. Response message: {responseMessage}");
            return null;
        }

        return await response.Content.ReadFromJsonAsync<Role>();
    }

    public async Task<UserRepresentation?> GetUser(Guid userId)
    {
        var response = await this.Client.GetAsync($"users/{userId}");
        if (!response.IsSuccessStatusCode)
        {
            var responseMessage = await response.Content.ReadAsStringAsync();
            this.Logger.LogError($"Could not retrieve user {userId} from Keycloak. Response message: {responseMessage}");
            return null;
        }

        return await response.Content.ReadFromJsonAsync<UserRepresentation>();
    }

    public async Task<bool> UpdateUser(Guid userId, UserRepresentation userRep)
    {
        var response = await this.Client.PutAsync($"users/{userId}", this.CreateStringContent(userRep));
        if (!response.IsSuccessStatusCode)
        {
            var responseMessage = await response.Content.ReadAsStringAsync();
            this.Logger.LogDebug($"Could not update the user {userId}. Response message: {responseMessage}");
            return false;
        }

        return true;
    }
}
