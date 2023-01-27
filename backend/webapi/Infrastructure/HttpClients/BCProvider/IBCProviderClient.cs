namespace Pidp.Infrastructure.HttpClients.BCProvider;

public interface IBCProviderClient
{
    /// <summary>
    /// Creates a BC Provider account on AAD
    /// Return the account object if the operation
    /// was successful.
    Task<BCProviderAccount> CreateBCProviderAccount(BCProviderAccount bCProviderAccount);

    /// <summary>
    /// Updates the password for a BC Provider account
    /// Returns true if the operation was successful
    Task<bool> UpdatePassword(string bcProviderId, string password);
}
