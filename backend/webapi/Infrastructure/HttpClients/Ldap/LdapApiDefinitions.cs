namespace Pidp.Infrastructure.HttpClients.Ldap;

public class LdapLoginRequest(string username, string password)
{
    public string UserName { get; set; } = username;
    public string Password { get; set; } = password;
}

// Possible response schemas:
// 1. User not found
// {}
//
// 2. User exists, correct password
// {
//     "authenticated": true,
//     "hcmuserrole": "<user's role>", (optional)
//     "unlocked": true,
//     "userName": "uid=<username>,o=<org name>",
//     "org_details": {
//         "id": "<id>",
//         "name": "<org name>"
//     } (optional)
// }
//
// 3. User exists but is locked, or bad password
// {
//     "authenticated": false,
//     "lockoutTimeInHours": 0, (unused)
//     "unlocked": <true or false>,
//     "remainingAttempts": <0 to 3>,
//     "userName": "uid=<username>,o=<org name>"
// }
public class LdapLoginResponse
{
    public bool? Authenticated { get; set; }
    public string? Hcmuserrole { get; set; }
    public int? RemainingAttempts { get; set; }
    public bool? Unlocked { get; set; }
    public string? UserName { get; set; }
#pragma warning disable CA1707 // Remove Underscores From Member Names
    public OrgDetails? Org_details { get; set; }
#pragma warning restore CA1707

    public bool UserNotFound => this.Authenticated == null;

    public class OrgDetails
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
