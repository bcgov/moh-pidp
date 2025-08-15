namespace Pidp.Infrastructure.HttpClients.BCProvider;

using Microsoft.Graph;
using Microsoft.Graph.Models;
using System.Text.RegularExpressions;

public partial class BCProviderClient(
    GraphServiceClient client,
    ILogger<BCProviderClient> logger,
    PidpConfiguration config) : IBCProviderClient
{
    private readonly GraphServiceClient client = client;
    private readonly ILogger<BCProviderClient> logger = logger;
    private readonly string appUrl = config.ApplicationUrl;
    private readonly string clientId = config.BCProviderClient.ClientId;
    private readonly string domain = config.BCProviderClient.Domain;

    public async Task<object?> GetAttribute(string userPrincipalName, string attributeName)
    {
        if (string.IsNullOrEmpty(userPrincipalName))
        {
            return null;
        }

        try
        {
            var result = await this.client.Users[userPrincipalName]
                .GetAsync(request => request.QueryParameters.Select = [attributeName]);

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
        var firstName = userRepresentation.FirstName;
        var userPrincipal = await this.CreateUniqueUserPrincipalName(firstName, userRepresentation.LastName);
        if (userPrincipal == null)
        {
            return null;
        }

        var bcProviderAccount = new User()
        {
            AccountEnabled = true,
            DisplayName = $"{firstName} {userRepresentation.LastName}", // Required
            GivenName = firstName,
            MailNickname = this.RemoveMailNicknameInvalidCharacters($"{firstName}{userRepresentation.LastName}"), // Required
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

    public async Task<string?> SendInvite(string userPrincipalName)
    {
        var invitation = new Invitation
        {
            InvitedUserEmailAddress = userPrincipalName,
            InviteRedirectUrl = this.appUrl,
            SendInvitationMessage = false,
            InvitedUserType = "Guest"
        };

        try
        {
            var response = await this.client.Invitations.PostAsync(invitation);
            if (response == null)
            {
                this.logger.LogInviteUserError(userPrincipalName, "Response was null.");
                return null;
            }
            else if (response.InvitedUser == null)
            {
                this.logger.LogInviteUserError(userPrincipalName, "InvitedUser was null.");
                return null;
            }
            else if (response.InvitedUser.Id == null)
            {
                this.logger.LogInviteUserError(userPrincipalName, "InvitedUser.ObjectId was null.");
                return null;
            }

            var upn = await this.GetUserPrincipalName(response.InvitedUser.Id);
            if (upn == null)
            {
                this.logger.LogInviteUserError(userPrincipalName, "InvitedUser.UserPrincipalName was null.");
            }
            return upn;
        }
        catch (ServiceException ex)
        {
            this.logger.LogInviteUserError(userPrincipalName, ex.Message);
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
        // We are attempting 3 times since Graph API does not support default auth method,
        // and attempting to delete the default auth method will result in an error.
        var maxAttempts = 3;
        var attempt = 0;
        while (!allMethodsDeleted && attempt < maxAttempts)
        {
            attempt++;
            var authMethods = await this.GetUserAuthMethods(userPrincipalName);
            if (authMethods?.Value is null)
            {
                // the user has no auth methods
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
                        switch (authMethod)
                        {
                            case EmailAuthenticationMethod:
                                await this.client.Users[userPrincipalName].Authentication.EmailMethods[authMethod.Id].DeleteAsync();
                                break;
                            case Fido2AuthenticationMethod:
                                await this.client.Users[userPrincipalName].Authentication.Fido2Methods[authMethod.Id].DeleteAsync();
                                break;
                            case MicrosoftAuthenticatorAuthenticationMethod:
                                await this.client.Users[userPrincipalName].Authentication.MicrosoftAuthenticatorMethods[authMethod.Id].DeleteAsync();
                                break;
                            case PhoneAuthenticationMethod:
                                await this.client.Users[userPrincipalName].Authentication.PhoneMethods[authMethod.Id].DeleteAsync();
                                break;
                            case SoftwareOathAuthenticationMethod:
                                await this.client.Users[userPrincipalName].Authentication.SoftwareOathMethods[authMethod.Id].DeleteAsync();
                                break;
                            case TemporaryAccessPassAuthenticationMethod:
                                await this.client.Users[userPrincipalName].Authentication.TemporaryAccessPassMethods[authMethod.Id].DeleteAsync();
                                break;
                            case WindowsHelloForBusinessAuthenticationMethod:
                                await this.client.Users[userPrincipalName].Authentication.WindowsHelloForBusinessMethods[authMethod.Id].DeleteAsync();
                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        this.logger.LogDeleteUserAuthMethodFailure(authMethod.OdataType, ex.Message);
                        allMethodsDeleted = false;
                    }
                }
            }
            catch (Exception ex)
            {
                this.logger.LogDeleteUserAuthMethodFailure(" possibly set as default auth method", ex.Message);
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

    private async Task<string?> CreateUniqueUserPrincipalName(string? firstName, string lastName)
    {
        var joinedFullName = string.IsNullOrEmpty(firstName) ? lastName : $"{firstName}.{lastName}";
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

    private async Task<string?> GetUserPrincipalName(string objectId)
    {
        var user = await this.client.Users[objectId]
            .GetAsync(request => request.QueryParameters.Select = ["userPrincipalName"]);

        return user?.UserPrincipalName;
    }

    private string RemoveMailNicknameInvalidCharacters(string mailNickname)
    {
        var validCharacters = InvalidMailNicknameRegex().Replace(mailNickname, string.Empty);

        if (mailNickname.Length != validCharacters.Length)
        {
            this.logger.LogPartyNameContainsMailNicknameInvalidCharacters(mailNickname, validCharacters);
        }

        return validCharacters;
    }

    private string RemoveUpnInvalidCharacters(string userPrincipalName)
    {
        var validCharacters = InvalidUserPrincipalNameRegex().Replace(userPrincipalName, string.Empty);

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
                request.QueryParameters.Filter = $"userPrincipalName eq '{userPrincipalName.Replace("'", "''")}'";
                request.Headers.Add("ConsistencyLevel", "eventual"); // Required for advanced queries such as "count"
            });

        return result > 0;
    }

    // Mail Nickname can include ASCII values 32 - 127 except the following: @ () \ [] " ; : . <> , SPACE
    [GeneratedRegex(@"[^a-zA-Z0-9!#$%&'*+\-\/=?\^_`{|}~]")]
    private static partial Regex InvalidMailNicknameRegex();

    // According to the Microsoft Graph docs, User Principal Name can only include A - Z, a - z, 0 - 9, and the characters ' . - _ ! # ^ ~
    [GeneratedRegex(@"[^a-zA-Z0-9'\.\-_!\#\^~]")]
    private static partial Regex InvalidUserPrincipalNameRegex();
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

    [LoggerMessage(12, LogLevel.Error, "An error occurred while deleting auth method {oDataType}: {message}")]
    public static partial void LogDeleteUserAuthMethodFailure(this ILogger<BCProviderClient> logger, string? oDataType, string message);

    [LoggerMessage(13, LogLevel.Error, "An error occurred while inviting user {userPrincipalName}. Reason: {reason}")]
    public static partial void LogInviteUserError(this ILogger<BCProviderClient> logger, string userPrincipalName, string? reason);
}
