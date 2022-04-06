namespace Pidp.Infrastructure.HttpClients.Ldap;

using DomainResults.Common;
using Microsoft.Extensions.Logging;

public class LdapClient : BaseClient, ILdapClient
{
    public LdapClient(HttpClient client, ILogger<LdapClient> logger) : base(client, logger) { }

    public async Task<IDomainResult<HcimLoginResult>> HcimLoginAsync(string username, string password)
    {
        var response = await this.PostAsync<HcimLoginResponse>("ldap/users", new LoginRequest(username, password));

        if (!response.IsSuccess)
        {
            return response.To<HcimLoginResult>();
        }

        return DomainResult.Success(HcimLoginResult.FromLoginResponse(response.Value));
    }
}

public class HcimLoginResult
{
    public enum AuthStatus
    {
        Success = 1,
        AccountLocked,
        AuthFailure,
        Unauthorized,
    }

    public AuthStatus Status { get; set; }
    public string? HcimUserRole { get; set; }
    public int? RemainingAttempts { get; set; }

    public bool IsError => this.Status != AuthStatus.Success;

    public static HcimLoginResult FromLoginResponse(HcimLoginResponse login)
    {
        if (login.Unlocked == false)
        {
            return new HcimLoginResult { Status = AuthStatus.AccountLocked };
        }

        if (login.Authenticated == true)
        {
            return new HcimLoginResult
            {
                HcimUserRole = login.Hcmuserrole,
                Status = string.IsNullOrWhiteSpace(login.Hcmuserrole)
                    ? AuthStatus.Unauthorized
                    : AuthStatus.Success
            };
        }

        return new HcimLoginResult
        {
            Status = AuthStatus.AuthFailure,
            RemainingAttempts = login.RemaingingAttempts
        };
    }
}

public static partial class LdapLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Error, "Empty response from LDAP login; most likely the user doesn't exist.")]
    public static partial void LogEmptyResponse(this ILogger logger);
}
