namespace Pidp.Extensions;

using Pidp.Features;

public static class IRequestCookieCollectionExtensions
{
    public static string? GetCredentialLinkTicket(this IRequestCookieCollection cookies) => cookies[PidpControllerBase.Cookies.CredentialLinkTicket.Key];
}
