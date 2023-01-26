namespace Pidp.Infrastructure.HttpClients.Aad;


public class AadClient : BaseClient, IAadClient
{
    public AadClient(HttpClient httpClient, ILogger<AadClient> logger) : base(httpClient, logger) { }

    public async Task<BcProviderAccount> CreateBcProviderAccount()
    {

    }

    public async Task<bool> UpdatePassword(string BcProviderId, string password)
    {

    }
}
