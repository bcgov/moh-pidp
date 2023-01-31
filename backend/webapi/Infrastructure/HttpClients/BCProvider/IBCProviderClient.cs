namespace Pidp.Infrastructure.HttpClients.BCProvider;

using Microsoft.Graph;

public interface IBCProviderClient
{
    /// <summary>
    /// Creates a BC Provider account on AAD
    /// Return the account object if the operation
    /// was successful.
    /// </summary>
    /// <param name="userRepresentation"></param>
    /// <returns></returns>
    Task<User> CreateBCProviderAccount(UserRepresentation userRepresentation);

    /// <summary>
    /// Updates the password for a BC Provider account
    /// Returns true if the operation was successful
    /// </summary>
    /// <param name="bcProviderId"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    Task<bool> UpdatePassword(string bcProviderId, string password);

    /// <summary>
    /// Checks if the user principal already exists
    /// </summary>
    /// <param name="userPrincipal"></param>
    /// <returns></returns>
    Task<bool> UserPrincipalExists(string userPrincipal);

    /// <summary>
    /// Creates the User Principal
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<string> CreateUserPrincipal(string name);

    /// <summary>
    /// Creates the User Principal. Only gets called
    /// if the name already exists
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<string> CreateUserPrincipalWithNumbers(string name);
}
