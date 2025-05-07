namespace Pidp.Infrastructure.HttpClients.BCProvider;

using Microsoft.Graph.Models;

public interface IBCProviderClient
{
    /// <summary>
    /// Creates a BC Provider account on AAD.
    /// Returns the account object if the operation was successful.
    /// </summary>
    /// <param name="userRepresentation"></param>
    public Task<User?> CreateBCProviderAccount(NewUserRepresentation userRepresentation);

    /// <summary>
    /// Gets an additional attribute for a BC Provider account.
    /// Returns null on an error.
    /// </summary>
    /// <param name="userPrincipalName"></param>
    /// <param name="attributesName"></param>
    public Task<object?> GetAttribute(string userPrincipalName, string attributeName);

    /// <summary>
    /// Removes all authentication methods from a user, except for password.
    /// Once all other auth methods are removed, the user will be prompted to
    /// re-register their MFA.
    /// </summary>
    /// <param name="userPrincipalName"></param>
    public Task<bool> RemoveAuthMethods(string userPrincipalName);

    /// <summary>
    /// Sends a Entra B2B guest account invite to an external Entra account.
    /// </summary>
    /// <param name="userPrincipalName"></param>
    /// <returns> ID of the created Guest user. </returns>
    public Task<string?> SendInvite(string userPrincipalName);

    /// <summary>
    /// Updates AAD attributes for a BC Provider account.
    /// Returns true if the operation was successful.
    /// </summary>
    /// <param name="userPrincipalName"></param>
    /// <param name="bcProviderAttributes">Set the value to null to remove the attribute</param>
    public Task<bool> UpdateAttributes(string userPrincipalName, IDictionary<string, object> bcProviderAttributes);

    /// <summary>
    /// Updates the password for a BC Provider account.
    /// Returns true if the operation was successful.
    /// </summary>
    /// <param name="userPrincipalName"></param>
    /// <param name="password"></param>
    public Task<bool> UpdatePassword(string userPrincipalName, string password);

    /// <summary>
    /// Patches the given BC Provider account.
    /// Returns true if the operation was successful.
    /// </summary>
    /// <param name="userPrincipalName"></param>
    /// <param name="user"></param>
    public Task<bool> UpdateUser(string userPrincipalName, User user);
}
