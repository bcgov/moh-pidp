namespace Pidp.Infrastructure.HttpClients.BCProvider;

using Microsoft.Graph;

public interface IBCProviderClient
{
    /// <summary>
    /// Creates a BC Provider account on AAD.
    /// Returns the account object if the operation was successful.
    /// </summary>
    /// <param name="userRepresentation"></param>
    Task<User?> CreateBCProviderAccount(NewUserRepresentation userRepresentation);

    /// <summary>
    /// Updates the password for a BC Provider account.
    /// Returns true if the operation was successful.
    /// </summary>
    /// <param name="userPrincipalName"></param>
    /// <param name="password"></param>
    Task<bool> UpdatePassword(string userPrincipalName, string password);
}
