namespace DoWork.Services.CredentialDeletionService;

public interface ICredentialDeletionService
{
    /// <summary>
    /// Deletes the Credentials from the database specified in the CredentialsToDelete file; one IdpID per line.
    /// </summary>
    public Task DeleteCredentialsAsync();
}
