namespace Pidp.Infrastructure.HttpClients.BCProvider;

using Microsoft.Graph;
using Microsoft.Graph.Models;

public class BCProviderClient : IBCProviderClient
{
    private readonly GraphServiceClient client;
    private readonly ILogger logger;
    private readonly string domain;
    private readonly string clientId;

    public BCProviderClient(
        GraphServiceClient client,
        ILogger<BCProviderClient> logger,
        PidpConfiguration config)
    {
        this.client = client;
        this.logger = logger;
        this.domain = config.BCProviderClient.Domain;
        this.clientId = config.BCProviderClient.ClientId;
    }

    public string GetAttributeKey(string attributeName) =>
        new BCProviderAttributes(this.clientId).GetBCProviderAttributeKey(attributeName);

    public async Task<IDictionary<string, object?>?> GetAttributes(string userPrincipalName)
    {
        try
        {
            if (string.IsNullOrEmpty(userPrincipalName))
            {
                return null;
            }
            var attributesName = new BCProviderAttributes(this.clientId).GetBCProviderAttributeKeys();

            var result = await this.client.Users[userPrincipalName]
                .GetAsync(request => request.QueryParameters.Select = attributesName);

            return result?.AdditionalData;
        }
        catch
        {
            this.logger.LogGetAdditionalAttributesFailure(userPrincipalName);
            return null;
        }
    }

    public async Task<User?> CreateBCProviderAccount(NewUserRepresentation userRepresentation)
    {
        var userPrincipal = await this.CreateUniqueUserPrincipalName(userRepresentation);
        if (userPrincipal == null)
        {
            return null;
        }

        var bcProviderAccount = new User()
        {
            AccountEnabled = true,
            DisplayName = userRepresentation.FullName, // Required
            GivenName = userRepresentation.FirstName,
            MailNickname = userRepresentation.FullName.Replace(" ", ""), // Required, cannot contain spaces
            Surname = userRepresentation.LastName,
            UserPrincipalName = userPrincipal,
            PasswordProfile = new PasswordProfile
            {
                ForceChangePasswordNextSignIn = false,
                Password = userRepresentation.Password
            },
            AdditionalData = BCProviderAttributes.FromNewUser(this.clientId, userRepresentation).AsAdditionalData()
        };

        try
        {
            var createdUser = await this.client.Users.PostAsync(bcProviderAccount);
            this.logger.LogNewBCProviderUserCreated(createdUser?.UserPrincipalName!);
            return createdUser;
        }
        catch
        {
            this.logger.LogAccountCreationFailure(userPrincipal);
            return null;
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="userPrincipalName"></param>
    /// <param name="bcProviderAttributes"></param>
    /// <returns></returns>
    public async Task<bool> UpdateAttributes(string userPrincipalName, IDictionary<string, object?> bcProviderAttributes)
    {
        try
        {
            await this.client.Users[userPrincipalName]
                .PatchAsync(new User
                {
                    AdditionalData = bcProviderAttributes
                });

            return true;
        }
        catch
        {
            this.logger.LogAttributesUpdateFailure(userPrincipalName);
            return false;
        }
    }

    public async Task<bool> UpdatePassword(string userPrincipalName, string password)
    {
        try
        {
            await this.client.Users[userPrincipalName]
                .PatchAsync(new User
                {
                    PasswordProfile = new PasswordProfile
                    {
                        ForceChangePasswordNextSignIn = false,
                        Password = password
                    }
                });

            return true;
        }
        catch
        {
            this.logger.LogPasswordUpdateFailure(userPrincipalName);
            return false;
        }
    }

    private async Task<string?> CreateUniqueUserPrincipalName(NewUserRepresentation user)
    {
        var joinedFullName = $"{user.FirstName}.{user.LastName}".Replace(" ", ""); // Cannot contain spaces.

        for (var i = 1; i <= 100; i++)
        {
            // Generates First.Last@domain instead of First.Last1@domain for the first instance of a name.
            var proposedName = $"{joinedFullName}{(i < 2 ? string.Empty : i)}@{this.domain}";
            if (!await this.UserExists(proposedName))
            {
                return proposedName;
            }
        }

        this.logger.LogNoUniqueUserPrincipalNameFound(joinedFullName);
        return null;
    }

    private async Task<bool> UserExists(string userPrincipalName)
    {
        var result = await this.client.Users.Count
            .GetAsync(request =>
            {
                request.QueryParameters.Filter = $"userPrincipalName eq '{userPrincipalName}'";
                request.Headers.Add("ConsistencyLevel", "eventual"); // Required for advanced queries such as "count"
            });

        return result > 0;
    }
}

public static partial class BCProviderClientLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Information, "Created new BC Provider user '{userPrincipalName}'.")]
    public static partial void LogNewBCProviderUserCreated(this ILogger logger, string userPrincipalName);

    [LoggerMessage(2, LogLevel.Error, "Failed to create account '{userPrincipalName}'.")]
    public static partial void LogAccountCreationFailure(this ILogger logger, string userPrincipalName);

    [LoggerMessage(4, LogLevel.Error, "Failed to update the password of user '{userPrincipalName}'.")]
    public static partial void LogPasswordUpdateFailure(this ILogger logger, string userPrincipalName);

    [LoggerMessage(5, LogLevel.Error, "Hit maximum retrys attempting to make a unique User Principal Name for user '{fullName}'.")]
    public static partial void LogNoUniqueUserPrincipalNameFound(this ILogger logger, string fullName);

    [LoggerMessage(6, LogLevel.Error, "Failed to update the attributes of user '{userPrincipalName}'.")]
    public static partial void LogAttributesUpdateFailure(this ILogger logger, string userPrincipalName);

    [LoggerMessage(7, LogLevel.Error, "Failed to get the attributes of user '{userPrincipalName}'.")]
    public static partial void LogGetAdditionalAttributesFailure(this ILogger logger, string userPrincipalName);
}
