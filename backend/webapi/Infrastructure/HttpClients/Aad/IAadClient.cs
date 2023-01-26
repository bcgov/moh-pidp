namespace Pidp.Infrastructure.HttpClients.Aad;

public interface IAadClient
{
    /// <summary>
    /// Creates a BC Provider account on AAD
    /// Return the account object if the operation
    /// was successful.
    Task<BcProviderAccount> CreateBcProviderAccount();

    /// <summary>
    /// Updates the password for a BC Provider account
    /// Returns true if the operation was successful
    Task<bool> UpdatePassword(string BcProviderId, string password);
}
