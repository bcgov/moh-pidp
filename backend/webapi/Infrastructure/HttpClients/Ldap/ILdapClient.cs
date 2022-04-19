namespace Pidp.Infrastructure.HttpClients.Ldap;

using DomainResults.Common;

public interface ILdapClient
{
    Task<IDomainResult<HcimAuthorizationStatus>> HcimLoginAsync(string username, string password);
}
