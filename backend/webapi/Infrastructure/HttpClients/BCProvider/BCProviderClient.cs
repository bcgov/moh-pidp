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
            this.logger.LogAccountCreationFailure(userPrincipal);
        }

        return null;
    }

    public async Task<bool> UpdatePassword(string bcProviderId, string password)
    {

        try
        {
            await this.client.Users["{bcProviderId}"].Authentication.PasswordMethods["{passwordAuthenticationMethod-id}"]
                .ResetPassword(password)
                .Request()
                .PostAsync();
        }
        catch
        {
            this.logger.LogPasswordUpdateFailure(bcProviderId);
        }

        return true;
    }

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
        var joinedFullName = $"{user.FirstName}.{user.LastName}".Replace(" ", ""); // Cannot contain spaces.

        for (var i = 1; i <= 100; i++)
        {
            // Generates First.Last@... instead of First.Last1@... for the first instance of a name.
            var proposedName = $"{joinedFullName}{(i < 2 ? string.Empty : i)}@bcproviderlab.ca";
            if (!await this.UserExists(proposedName))
            {
                return proposedName;
            }
        }

        throw new Exception("we should do something if there are more than 100 people with the same name");
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

public static partial class BCProviderLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Error, "Failed to create account {userPrincipal}.")]
    public static partial void LogAccountCreationFailure(this ILogger logger, string userPrincipal);

    [LoggerMessage(2, LogLevel.Error, "Failed to update user {bcProviderId}'s password.")]
    public static partial void LogPasswordUpdateFailure(this ILogger logger, string bcProviderId);
}
