namespace Pidp.Infrastructure.HttpClients.BCProvider;

using Microsoft.Graph;

public class BCProviderClient : IBCProviderClient
{
    private readonly GraphServiceClient client;
    private readonly ILogger logger;
    private readonly string domain;

    public BCProviderClient(
        GraphServiceClient client,
        ILogger<BCProviderClient> logger,
        PidpConfiguration config)
    {
        this.client = client;
        this.logger = logger;
        this.domain = config.BCProviderClient.Domain;
    }

    public async Task<User?> CreateBCProviderAccount(UserRepresentation userRepresentation)
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
            AdditionalData = new Dictionary<string, object>
            {
                {
                    "bcproviderlab_additionalAttributes" , new
                    {
                        LOA = 3,
                        HPDID = "thisisafakehpdid"
                    }
                },
            },
        };

        try
        {
            var user = await this.client.Users.Request().AddAsync(bcProviderAccount);
            this.logger.LogNewBCProviderUserCreated(user.UserPrincipalName);
            return user;
        }
        catch
        {
            this.logger.LogAccountCreationFailure(userPrincipal);
            return null;
        }
    }

    public async Task<bool> UpdatePassword(string userPrincipalName, string password)
    {
        try
        {
            await this.client.Users[userPrincipalName]
                .Request()
                .UpdateAsync(new User
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

    public async Task<SchemaExtension?> RegisterSchemaExtension()
    {
        var schemaExtension = new SchemaExtension
        {
            Id = "bcproviderlab_additionalAttributes",
            Description = "Additional User attributes",
            TargetTypes = new List<string>
            {
                "user",
            },
            Properties = new List<ExtensionSchemaProperty>
            {
                new ExtensionSchemaProperty
                {
                    Name = "LOA",
                    Type = "Integer",
                },
                new ExtensionSchemaProperty
                {
                    Name = "HPDID",
                    Type = "String",
                },
            },
        };

        try
        {
            var result = await this.client.SchemaExtensions.Request().AddAsync(schemaExtension);
            // TODO Do logging
            return result;
        }
        catch
        {
            // TODO Do logging
            return null;
        }
    }

    private async Task<string?> CreateUniqueUserPrincipalName(UserRepresentation user)
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
        var result = await this.client.Users.Request()
            .Select("UserPrincipalName")
            .Filter($"UserPrincipalName eq '{userPrincipalName}'")
            .GetAsync();

        return result.Count > 0;
    }
}

public static partial class BCProviderLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Information, "Created new BC Provider user '{userPrincipalName}'.")]
    public static partial void LogNewBCProviderUserCreated(this ILogger logger, string userPrincipalName);

    [LoggerMessage(2, LogLevel.Error, "Failed to create account '{userPrincipalName}'.")]
    public static partial void LogAccountCreationFailure(this ILogger logger, string userPrincipalName);

    [LoggerMessage(4, LogLevel.Error, "Failed to update the password of user '{userPrincipalName}'.")]
    public static partial void LogPasswordUpdateFailure(this ILogger logger, string userPrincipalName);

    [LoggerMessage(5, LogLevel.Error, "Hit maximum retrys attempting to make a unique User Principal Name for user '{fullName}'.")]
    public static partial void LogNoUniqueUserPrincipalNameFound(this ILogger logger, string fullName);
}
