namespace Pidp.Infrastructure.HttpClients.BCProvider;


public class BCProviderClient : BaseClient, IBCProviderClient
{
    public BCProviderClient(HttpClient httpClient, ILogger<BCProviderClient> logger) : base(httpClient, logger) { }

    public async Task<BCProviderAccount> CreateBCProviderAccount()
    {

    }

    public async Task<bool> UpdatePassword(string BCProviderId, string password)
    {

    }
}
