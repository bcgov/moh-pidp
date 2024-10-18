namespace Pidp.Infrastructure.HttpClients.Ldap;

using DomainResults.Common;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

public class LdapClient(HttpClient client, ILogger<LdapClient> logger) : BaseClient(client, logger), ILdapClient
{
    public async Task<IDomainResult<HcimAuthorizationStatus>> HcimLoginAsync(string username, string password)
    {
        var response = await this.PostAsync<LdapLoginResponse>("ldap/users", new LdapLoginRequest(username, password));

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
    public LdapLoginResponse.OrgDetails? OrgDetails { get; set; }
    public int? RemainingAttempts { get; set; }

    [MemberNotNullWhen(true, nameof(HcimUserRole), nameof(OrgDetails))]
    public bool IsAuthorized => this.Status == AuthorizationStatus.Authorized;

    public static HcimAuthorizationStatus FromLoginResponse(LdapLoginResponse login)
    {
        if (login.Unlocked == false)
        {
            return new HcimAuthorizationStatus { Status = AuthorizationStatus.AccountLocked };
        }

        if (login.Authenticated == true)
        {
            if (string.IsNullOrWhiteSpace(login.Hcmuserrole))
            {
                return new HcimAuthorizationStatus { Status = AuthorizationStatus.Unauthorized };
            }

            return new HcimAuthorizationStatus
            {
                Status = AuthorizationStatus.Authorized,
                HcimUserRole = login.Hcmuserrole,
                OrgDetails = login.Org_details,
            };
        }

        return new HcimAuthorizationStatus
        {
            Status = AuthorizationStatus.AuthFailure,
            RemainingAttempts = login.RemainingAttempts
        };
    }
}
