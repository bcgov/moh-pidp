namespace Pidp.Infrastructure.HttpClients.Ldap;

using DomainResults.Common;
using Microsoft.Extensions.Logging;

public class LdapClient : BaseClient, ILdapClient
{
    public LdapClient(HttpClient client, ILogger<LdapClient> logger) : base(client, logger) { }

    public async Task<IDomainResult<HcimAuthorizationStatus>> HcimLoginAsync(string username, string password)
    {
        var response = await this.PostAsync<LdapLoginResponse>("ldap/users", new LoginRequest(username, password));

        if (!response.IsSuccess)
        {
            return response.To<HcimAuthorizationStatus>();
        }

        return DomainResult.Success(HcimAuthorizationStatus.FromLoginResponse(response.Value));
    }
}

public class HcimAuthorizationStatus
{
    public enum AuthorizationStatus
    {
        Authorized = 1,
        AccountLocked,
        AuthFailure,
        Unauthorized,
    }

    public AuthorizationStatus Status { get; set; }
    public string? HcimUserRole { get; set; }
    public int? RemainingAttempts { get; set; }

    public bool IsAuthorized => this.Status == AuthorizationStatus.Authorized;

    public static HcimAuthorizationStatus FromLoginResponse(LdapLoginResponse login)
    {
        if (login.Unlocked == false)
        {
            return new HcimAuthorizationStatus { Status = AuthorizationStatus.AccountLocked };
        }

        if (login.Authenticated == true)
        {
            return new HcimAuthorizationStatus
            {
                HcimUserRole = login.Hcmuserrole,
                Status = string.IsNullOrWhiteSpace(login.Hcmuserrole)
                    ? AuthorizationStatus.Unauthorized
                    : AuthorizationStatus.Authorized
            };
        }

        return new HcimAuthorizationStatus
        {
            Status = AuthorizationStatus.AuthFailure,
            RemainingAttempts = login.RemaingingAttempts
        };
    }
}
