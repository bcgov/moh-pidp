namespace Pidp.Infrastructure.HttpClients.BCProvider;

using Azure.Identity;
using Microsoft.Graph;
using static Pidp.PidpConfiguration;

public class BCProviderClient : HttpClients.BaseClient, IBCProviderClient
{
    private readonly BCProviderClientConfiguration config;
    public BCProviderClient(
        HttpClient httpClient,
        ILogger<BCProviderClient> logger,
        PidpConfiguration config) : base(httpClient, logger) => this.config = config.BCProviderClient;

    public async Task<User> CreateBCProviderAccount(UserRepresentation userRepresentation)
    {
        var clientId = this.config.ClientId;

        var tenantId = this.config.TenantId;

        var clientSecret = this.config.ClientSecret;

        // Scope construction from here:
        // https://learn.microsoft.com/en-us/azure/active-directory/develop/scenario-daemon-acquire-token?tabs=dotnet
        //var scopes = new[] { "User.ReadWrite.All/.default" };//, "Directory.ReadWrite.All" };
        var scopes = new string[] { "https://graph.microsoft.com/.default" };
        var options = new TokenCredentialOptions
        {
            AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
        };

        var credential = new ClientSecretCredential(tenantId, clientId, clientSecret, options);
        var client = new GraphServiceClient(credential, scopes);

        // NOTE: This must match an allowed domain as configured in Azure or AddAsync() below will fail.
        // For how to view the domains:
        // https://learn.microsoft.com/en-us/azure/active-directory/enterprise-users/domains-manage
        var userPrincipal = $"{userRepresentation.FullName}@bcproviderlab.ca";

        // NOTE: These is the minimum set of properties that must be set for the user creation to work.
        var bcProviderAccount = new User()
        {
            AccountEnabled = true,
            DisplayName = userRepresentation.FullName,
            MailNickname = userRepresentation.FullName,
            UserPrincipalName = userPrincipal,
            PasswordProfile = new PasswordProfile
            {
                ForceChangePasswordNextSignIn = true,
                Password = "Graph-Spike"
            }
        };

        var result = await client.Users.Request().AddAsync(bcProviderAccount);
        return result;
    }

    public async Task<bool> UpdatePassword(string bcProviderId, string password)
    {

    }
}
