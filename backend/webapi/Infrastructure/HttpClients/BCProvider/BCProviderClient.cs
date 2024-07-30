namespace Pidp.Infrastructure.HttpClients.BCProvider;

using Microsoft.Graph;
using Microsoft.Graph.Models;
using System.Text.RegularExpressions;

public class BCProviderClient : IBCProviderClient
{
    private readonly GraphServiceClient client;
    private readonly ILogger<BCProviderClient> logger;
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

    public async Task<object?> GetAttribute(string userPrincipalName, string attributeName)
    {
        if (string.IsNullOrEmpty(userPrincipalName))
        {
            return null;
        }

        try
        {
            var result = await this.client.Users[userPrincipalName]
                .GetAsync(request => request.QueryParameters.Select = new[] { attributeName });

            return result?.AdditionalData[attributeName];
        }
        catch
        {
            this.logger.LogGetAttributeFailure(userPrincipalName);
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
            DisplayName = $"{userRepresentation.FirstName} {userRepresentation.LastName}", // Required
            GivenName = userRepresentation.FirstName,
            MailNickname = this.RemoveMailNicknameInvalidCharacters($"{userRepresentation.FirstName}{userRepresentation.LastName}"), // Required
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
        catch (Exception e)
        {
            this.logger.LogAccountCreationFailure(userPrincipal, e);
            return null;
        }
    }

    public async Task<bool> UpdateAttributes(string userPrincipalName, IDictionary<string, object> bcProviderAttributes)
    {
        if (bcProviderAttributes.Count == 0)
        {
            return true;
        }

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

    public async Task<bool> UpdateUser(string userPrincipalName, User user)
    {
        try
        {
            await this.client.Users[userPrincipalName]
                .PatchAsync(user);

            return true;
        }
        catch
        {
            this.logger.LogUserUpdateFailure(userPrincipalName);
            return false;
        }
    }


    public async Task<bool> RemoveAuthMethods(string userPrincipalName)
    {
        var allMethodsDeleted = false;
        // This is a workaround for the re-register MFA feature that does not exist in the Graph API,
        // so we will delete all auth methods which will prompt the user to re-register.
        // We are attempting 3 times since have no access to know which is the default auth method,
        // and attempting to delete the default auth method will result in an error.
        var maxAttempts = 3;
        var attempt = 0;
        while (!allMethodsDeleted && attempt < maxAttempts)
        {
            attempt++;
            var authMethods = await this.GetUserAuthMethods(userPrincipalName);
            if (authMethods?.Value is null)
            {
                //TODO If the user has no auth methods, what?
                // I guess our job is done here?
                return true;
            }
            // We cannot delete the PasswordAuthenticationMethod
            var filteredAuthMethods = authMethods.Value.Where(authMethod => authMethod is not PasswordAuthenticationMethod).ToList();
            if (filteredAuthMethods.Count == 0)
            {
                allMethodsDeleted = true;
                break;
            }
            try
            {
                foreach (var authMethod in filteredAuthMethods)
                {
                    // If the request returns an error, it's possible that the auth method is default and cannot be deleted.
                    // In this case, we want to continue deleting the other auth methods.
                    try
                    {
                        //TODO: Do we even need to check ALL of these? wait for confirmation
                        switch (authMethod)
                        {
                            case EmailAuthenticationMethod:
                                Console.WriteLine("Email auth method found, delete it.");
                                await this.client.Users[userPrincipalName].Authentication.EmailMethods[authMethod.Id].DeleteAsync();
                                break;
                            case Fido2AuthenticationMethod:
                                Console.WriteLine("FIDO auth method found, delete it.");
                                await this.client.Users[userPrincipalName].Authentication.Fido2Methods[authMethod.Id].DeleteAsync();
                                break;
                            case MicrosoftAuthenticatorAuthenticationMethod:
                                Console.WriteLine("Microsoft authenticator auth method found, delete it.");
                                await this.client.Users[userPrincipalName].Authentication.MicrosoftAuthenticatorMethods[authMethod.Id].DeleteAsync();
                                break;
                            case PhoneAuthenticationMethod:
                                Console.WriteLine("Phone auth method found, delete it.");
                                await this.client.Users[userPrincipalName].Authentication.PhoneMethods[authMethod.Id].DeleteAsync();
                                break;
                            case SoftwareOathAuthenticationMethod:
                                Console.WriteLine("Oath auth method found, delete it.");
                                await this.client.Users[userPrincipalName].Authentication.SoftwareOathMethods[authMethod.Id].DeleteAsync();
                                break;
                            case TemporaryAccessPassAuthenticationMethod:
                                Console.WriteLine("Temp access pass auth method found, delete it.");
                                await this.client.Users[userPrincipalName].Authentication.TemporaryAccessPassMethods[authMethod.Id].DeleteAsync();
                                break;
                            case WindowsHelloForBusinessAuthenticationMethod:
                                Console.WriteLine("Windows Hello for Business auth method found, delete it.");
                                await this.client.Users[userPrincipalName].Authentication.WindowsHelloForBusinessMethods[authMethod.Id].DeleteAsync();
                                break;
                            default:
                                Console.WriteLine("What? No auth method found?");
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred while deleting auth method {authMethod.OdataType}: {ex.Message}");
                        allMethodsDeleted = false;
                    }
                }
                if (filteredAuthMethods.Count == 0)
                {
                    allMethodsDeleted = true;
                    break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return false;
            }
        }
        return allMethodsDeleted;
    }

    private async Task<AuthenticationMethodCollectionResponse?> GetUserAuthMethods(string userPrincipalName)
    {
        try
        {
            var authMethods = await this.client.Users[userPrincipalName].Authentication.Methods
                .GetAsync();

            return authMethods;
        }
        catch
        {
            this.logger.LogGetUserAuthMethodsFailure(userPrincipalName);
            return null;
        }
    }

    private async Task<string?> CreateUniqueUserPrincipalName(NewUserRepresentation user)
    {
        var joinedFullName = $"{user.FirstName}.{user.LastName}";
        var validCharacters = this.RemoveUpnInvalidCharacters(joinedFullName);

        for (var i = 1; i <= 100; i++)
        {
            // Generates First.Last@domain instead of First.Last1@domain for the first instance of a name.
            var proposedName = $"{validCharacters}{(i < 2 ? string.Empty : i)}@{this.domain}";
            if (!await this.UserExists(proposedName))
            {
                return proposedName;
            }
        }

        this.logger.LogNoUniqueUserPrincipalNameFound(validCharacters);
        return null;
    }

    private string RemoveMailNicknameInvalidCharacters(string mailNickname)
    {
        // Mail Nickname can include ASCII values 32 - 127 except the following: @ () \ [] " ; : . <> , SPACE
        var validCharacters = Regex.Replace(mailNickname, @"[^a-zA-Z0-9!#$%&'*+\-\/=?\^_`{|}~]", string.Empty);

        if (mailNickname.Length != validCharacters.Length)
        {
            this.logger.LogPartyNameContainsMailNicknameInvalidCharacters(mailNickname, validCharacters);
        }

        return validCharacters;
    }

    private string RemoveUpnInvalidCharacters(string userPrincipalName)
    {
        // According to the Microsoft Graph docs, User Principal Name can only include A - Z, a - z, 0 - 9, and the characters ' . - _ ! # ^ ~
        var validCharacters = Regex.Replace(userPrincipalName, @"[^a-zA-Z0-9'\.\-_!\#\^~]", string.Empty);

        if (userPrincipalName.Length != validCharacters.Length)
        {
            this.logger.LogPartyNameContainsUpnInvalidCharacters(userPrincipalName, validCharacters);
        }

        return validCharacters;
    }

    private async Task<bool> UserExists(string userPrincipalName)
    {
        var result = await this.client.Users.Count
            .GetAsync(request =>
            {
                request.QueryParameters.Filter = GetQueryParametersFilter(userPrincipalName);
                request.Headers.Add("ConsistencyLevel", "eventual"); // Required for advanced queries such as "count"
            });

        return result > 0;
    }

    private static string GetQueryParametersFilter(string userPrincipalName)
    {
        var searchValue = Regex.Replace(userPrincipalName, "'", "''");
        return $"userPrincipalName eq '{searchValue}'";
    }
}

public static partial class BCProviderClientLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Information, "Created new BC Provider user '{userPrincipalName}'.")]
    public static partial void LogNewBCProviderUserCreated(this ILogger<BCProviderClient> logger, string userPrincipalName);

    [LoggerMessage(2, LogLevel.Error, "Failed to create account '{userPrincipalName}'.")]
    public static partial void LogAccountCreationFailure(this ILogger<BCProviderClient> logger, string userPrincipalName, Exception e);

    [LoggerMessage(4, LogLevel.Error, "Failed to update the password of user '{userPrincipalName}'.")]
    public static partial void LogPasswordUpdateFailure(this ILogger<BCProviderClient> logger, string userPrincipalName);

    [LoggerMessage(5, LogLevel.Error, "Hit maximum retrys attempting to make a unique User Principal Name for user '{fullName}'.")]
    public static partial void LogNoUniqueUserPrincipalNameFound(this ILogger<BCProviderClient> logger, string fullName);

    [LoggerMessage(6, LogLevel.Error, "Failed to update the attributes of user '{userPrincipalName}'.")]
    public static partial void LogAttributesUpdateFailure(this ILogger<BCProviderClient> logger, string userPrincipalName);

    [LoggerMessage(7, LogLevel.Error, "Failed to get an attribute of user '{userPrincipalName}'.")]
    public static partial void LogGetAttributeFailure(this ILogger<BCProviderClient> logger, string userPrincipalName);

    [LoggerMessage(8, LogLevel.Warning, "Party's full name contained characters invalid for an AAD Mail Nickname. '{partyFullName}' was shortened to '{partyShortenedName}'.")]
    public static partial void LogPartyNameContainsMailNicknameInvalidCharacters(this ILogger<BCProviderClient> logger, string partyFullName, string partyShortenedName);

    [LoggerMessage(9, LogLevel.Warning, "Party's full name contained characters invalid for an AAD User Principal Name. '{partyFullName}' was shortened to '{partyShortenedName}'.")]
    public static partial void LogPartyNameContainsUpnInvalidCharacters(this ILogger<BCProviderClient> logger, string partyFullName, string partyShortenedName);

    [LoggerMessage(10, LogLevel.Error, "Failed to update the user '{userPrincipalName}'.")]
    public static partial void LogUserUpdateFailure(this ILogger<BCProviderClient> logger, string userPrincipalName);

    [LoggerMessage(11, LogLevel.Error, "Failed to retrieve authentication methods for the user '{userPrincipalName}'.")]
    public static partial void LogGetUserAuthMethodsFailure(this ILogger<BCProviderClient> logger, string userPrincipalName);
}
