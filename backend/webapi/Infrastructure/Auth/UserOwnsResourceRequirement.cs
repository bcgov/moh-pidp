namespace Pidp.Infrastructure.Auth;

using Microsoft.AspNetCore.Authorization;

using Pidp.Extensions;
using Pidp.Models;

public class UserOwnsResourceRequirement : IAuthorizationRequirement { }

public class UserOwnsResourceHandler : AuthorizationHandler<UserOwnsResourceRequirement, IOwnedResource>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserOwnsResourceRequirement requirement, IOwnedResource resource)
    {
        if (resource == null)
        {
            // TODO or error? Re-evaluate if auth gets more complicated.
            return Task.CompletedTask;
        }

        var userId = context.User.GetUserId();
        if (userId != Guid.Empty
            && userId == resource.UserId)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
