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
    /// Get attribute key for a attribute name.
    /// Example: "isMd" will return "Extension_{unique_identifier}_isMd".
    /// </summary>
    /// <param name="attributeName"></param>
    /// <returns></returns>
    string GetAdditionalAttributeKey(string attributeName);

    /// <summary>
    /// Get all additional attributes for a BC Provider account.
    /// </summary>
    /// <param name="userPrincipalName"></param>
    /// <returns>If the BC provider account is not found return null</returns>
    Task<IDictionary<string, object?>?> GetAdditionalAttributes(string userPrincipalName);

    /// <summary>
    /// Updates AAD attributes for a BC Provider account.
    /// Returns true if the operation was successful.
    /// </summary>
    /// <param name="userPrincipalName"></param>
    /// <param name="bcProviderAttributes"></param>
    Task<bool> UpdateAttributes(string userPrincipalName, IDictionary<string, object?> bcProviderAttributes);

    /// <summary>
    /// Updates the password for a BC Provider account.
    /// Returns true if the operation was successful.
    /// </summary>
    /// <param name="userPrincipalName"></param>
    /// <param name="password"></param>
    Task<bool> UpdatePassword(string userPrincipalName, string password);
}
