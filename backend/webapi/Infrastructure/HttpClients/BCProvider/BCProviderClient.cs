namespace Pidp.Infrastructure.HttpClients.BCProvider;

using Microsoft.Graph;

public class BCProviderClient : IBCProviderClient
{
    private readonly GraphServiceClient client;
    private readonly ILogger logger;

    public BCProviderClient(GraphServiceClient client, ILogger<BCProviderClient> logger)
    {
        this.client = client;
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
                Password = userRepresentation.Password
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

    private async Task<string?> CreateUniqueUserPrincipalName(UserRepresentation user)
    {
        var joinedFullName = $"{user.FirstName}.{user.LastName}".Replace(" ", ""); // Cannot contain spaces.

        for (var i = 1; i <= 100; i++)
        {
            // Generates First.Last@domain instead of First.Last1@domain for the first instance of a name.
            var proposedName = $"{joinedFullName}{(i < 2 ? string.Empty : i)}@bcproviderlab.ca";
            if (!await this.UserExists(proposedName))
            {
                this.logger.LogNewBCProviderUserCreated(proposedName);
                return proposedName;
            }
        }
        this.logger.LogBCProviderAccountCreationFailure(joinedFullName, user.FirstName, user.LastName);
        return null;
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

    [LoggerMessage(3, LogLevel.Information, "Created new BC Provider user {proposedName}")]
    public static partial void LogNewBCProviderUserCreated(this ILogger logger, string proposedName);

    [LoggerMessage(4, LogLevel.Error, "Failed to create user {fullName}, possibly reached maximum amount of users named {firstName} {lastName}.")]
    public static partial void LogBCProviderAccountCreationFailure(this ILogger logger, string fullName, string firstName, string lastName);
}
