namespace Pidp.Infrastructure.HttpClients.Ldap;

using DomainResults.Common;

public interface ILdapClient
{
    Task<IDomainResult<HcimLoginResult>> HcimLoginAsync(string username, string password);
}
