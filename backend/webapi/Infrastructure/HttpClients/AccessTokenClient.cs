namespace Pidp.Infrastructure.HttpClients;

using IdentityModel.Client;

public class AccessTokenClient(HttpClient client) : IAccessTokenClient
{
    private readonly HttpClient client = client ?? throw new ArgumentNullException(nameof(client));

    public async Task<string> GetAccessTokenAsync(ClientCredentialsTokenRequest request)
    {
        var response = await this.client.RequestClientCredentialsTokenAsync(request);
        return response.AccessToken!;
    }
}
