namespace Pidp.Infrastructure.HttpClients.BCProvider;

using Microsoft.Graph.Models;

public interface IBCProviderClient
{
    /// <summary>
    /// Creates a BC Provider account on AAD.
    /// Returns the account object if the operation was successful.
    /// </summary>
    /// <param name="userRepresentation"></param>
    Task<User?> CreateBCProviderAccount(NewUserRepresentation userRepresentation);

    /// <summary>
    /// Gets an additional attribute for a BC Provider account.
    /// Returns null on an error.
    /// </summary>
    /// <param name="userPrincipalName"></param>
    /// <param name="attributesName"></param>
    Task<object?> GetAttribute(string userPrincipalName, string attributeName);

    /// <summary>
    /// Updates AAD attributes for a BC Provider account.
    /// Returns true if the operation was successful.
    /// </summary>
    /// <param name="userPrincipalName"></param>
    /// <param name="bcProviderAttributes">Set the value to null to remove the attribute</param>
    Task<bool> UpdateAttributes(string userPrincipalName, IDictionary<string, object> bcProviderAttributes);

    /// <summary>
    /// Updates the password for a BC Provider account.
    /// Returns true if the operation was successful.
    /// </summary>
    /// <param name="userPrincipalName"></param>
    /// <param name="password"></param>
    Task<bool> UpdatePassword(string userPrincipalName, string password);

    /// <summary>
    /// Patches the given BC Provider account.
    /// Returns true if the operation was successful.
    /// </summary>
    /// <param name="userPrincipalName"></param>
    /// <param name="user"></param>
    Task<bool> UpdateUser(string userPrincipalName, User user);

    /// <summary>
    /// Deletes the BC Provider account.
    /// Only used specifically when the keycloak account associated with the BC Provider account
    /// fails to create. This deletes the BC Provider account in AAD to prevent orphaned accounts.
    /// </summary>
    /// <param name="userPrincipalName"></param>

    Task<bool> DeleteBCProviderAccount(string userPrincipalName);
}
