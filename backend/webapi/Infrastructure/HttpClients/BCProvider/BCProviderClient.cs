namespace Pidp.Infrastructure.HttpClients.BCProvider;


public class BCProviderClient : BaseClient, IBCProviderClient
{
    public BCProviderClient(HttpClient httpClient, ILogger<BCProviderClient> logger) : base(httpClient, logger) { }

    public async Task<BcProviderAccount> CreateBcProviderAccount()
    {

    }

    public async Task<bool> UpdatePassword(string BcProviderId, string password)
    {

    }
}
