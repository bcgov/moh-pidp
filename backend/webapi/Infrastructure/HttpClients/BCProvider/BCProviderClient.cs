namespace Pidp.Infrastructure.HttpClients.BCProvider;

using Azure.Identity;
using Microsoft.Graph;
using static Pidp.PidpConfiguration;

public class BCProviderClient : IBCProviderClient
{
    private readonly GraphServiceClient client;
    private readonly ILogger logger;

    public BCProviderClient(ILogger<BCProviderClient> logger, PidpConfiguration config)
    {
        this.client = BuildClient(config.BCProviderClient);
        this.logger = logger;
    }

    public async Task<User?> CreateBCProviderAccount(UserRepresentation userRepresentation)
    {
        var userPrincipal = await this.CreateUniqueUserPrincipalName(userRepresentation);

        // NOTE: These is the minimum set of properties that must be set for the user creation to work.
        var bcProviderAccount = new User()
        {
            AccountEnabled = true,
            DisplayName = userRepresentation.FullName,
            MailNickname = userRepresentation.FullName.Replace(" ", ""), // Cannot contain spaces
            UserPrincipalName = userPrincipal,
            PasswordProfile = new PasswordProfile
            {
                Password = "Graph-Spike"
                // Password = userRepresentation.Password
            }
        };

        try
        {
            return await this.client.Users.Request().AddAsync(bcProviderAccount);
        }
        catch
        {
            // TODO: logging
        }

        return null;
    }

    public async Task<bool> UpdatePassword(string bcProviderId, string password)
    {

        try
        {
            await client.Users["{bcProviderId}"].Authentication.PasswordMethods["{passwordAuthenticationMethod-id}"]
                .ResetPassword(password)
                .Request()
                .PostAsync();
        }
        catch
        {
            // TODO: logging
        }

    }

    private static string CreateUserPrincipalWithNumbers(string name) => $"{name}@bcproviderlab$" + "{Next(1, 99)}" + ".ca";

    private static GraphServiceClient BuildClient(BCProviderClientConfiguration config)
    {
        var scopes = new string[] { "https://graph.microsoft.com/.default" };
        var options = new TokenCredentialOptions
        {
            AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
        };
        var credential = new ClientSecretCredential(config.TenantId, config.ClientId, config.ClientSecret, options);

        // TODO: GraphServiceClient creates a new HttpClient using GraphClientFactory that has custom headers and middleware.
        // We should be injecting a managed HttpClient rather than creating a new one every time (to avoid socket exhaustion) but then it won't have that custom configuration.
        // Review this if/when Microsoft.Graph gives guidance on the propper pattern for doing this.
        return new GraphServiceClient(credential, scopes);
    }

    private async Task<string> CreateUniqueUserPrincipalName(UserRepresentation user)
    {
        var name = user.FullName.Replace(" ", "");

        var userPrincipal = $"{name}@bcproviderlab$" + ".ca";

        while (await this.UserExists(userPrincipal))
        {
            userPrincipal = CreateUserPrincipalWithNumbers(name);
        }

        return userPrincipal;
    }

    private async Task<bool> UserExists(string userPrincipalName)
    {
        var result = await this.client.Users.Request()
            .Select("UserPrincipalName")
            .Filter($"UserPrincipalName eq '{userPrincipalName}'")
            .GetAsync();

        return result.Count > 0;
    }
}
