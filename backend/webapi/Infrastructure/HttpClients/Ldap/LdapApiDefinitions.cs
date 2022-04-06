namespace Pidp.Infrastructure.HttpClients.Ldap;

public class LoginRequest
{
    public string UserName { get; set; }
    public string Password { get; set; }

    public LoginRequest(string username, string password)
    {
        this.UserName = username;
        this.Password = password;
    }
}

// Possible response schemas:
// 1. User not found
// {}
//
// 2. User exists, correct password
// {
//     "authenticated": true,
//     ["hcmuserrole": "<user's role>",]
//     "unlocked": true,
//     "userName": "uid=<username>,o=Ministry of Health"
// }
//
// 3. User exists but is locked, or bad password
// {
//     "authenticated": false,
//     "lockoutTimeInHours": 0, (unused)
//     "unlocked": <true or false>,
//     "remainingAttempts": <0 to 3>,
//     "userName": "uid=<username>,o=Ministry of Health"
// }
public class HcimLoginResponse
{
    public bool? Authenticated { get; set; }
    public string? Hcmuserrole { get; set; }
    public int? RemaingingAttempts { get; set; }
    public bool? Unlocked { get; set; }
    public string? UserName { get; set; }

    public bool UserNotFound => this.Authenticated == null;
}
