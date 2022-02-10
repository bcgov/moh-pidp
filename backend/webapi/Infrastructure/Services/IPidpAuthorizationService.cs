namespace Pidp.Infrastructure.Services;

using DomainResults.Common;
using System.Linq.Expressions;
using System.Security.Claims;

using Pidp.Infrastructure.Auth;
using Pidp.Models;

public interface IPidpAuthorizationService
{
    /// <summary>
    /// Checks that the given Resource both exists and can be accessed by the given User, based on the given Policy.
    /// </summary>
    /// <typeparam name="T">The Type of the Resource</typeparam>
    /// <param name="predicate">Filtering predicate to find the Resource from the DB; usually an Id check eg: (Party p) => p.Id == partyId</param>
    /// <param name="user">The User to authorize agains</param>
    /// <param name="policy">The Authorization policy to authorize against; defaults to UserOwnsResource (User has same UserId as the record)</param>
    Task<IDomainResult> CheckResourceAccessibility<T>(Expression<Func<T, bool>> predicate, ClaimsPrincipal user, string policy = Policies.UserOwnsResource) where T : class, IOwnedResource;
}
