namespace Pidp.Infrastructure.HttpClients.BCProvider;

// shape of object sent to AAD to create the account
// or
// shape of account information returned after creation?
public class BcProviderAccount
{
    public string DisplayName { get; set; }
    public string MailNickname { get; set; }
    public string UserPrincipalName { get; set; }
    public bool AccountEnabled { get; set; }
    public PasswordProfile PasswordProfile { get; set; }
}

public class PasswordProfile
{
    public bool ForceChangePasswordNextSignIn { get; set; }
    public string Password { get; set; }
}
