namespace Pidp.Infrastructure.Auth;

using Microsoft.AspNetCore.Authorization;
using Pidp.Features;

public class UserOwnsPartyHandler : AuthorizationHandler<UserOwnsPartyRequirement, int>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserOwnsPartyRequirement requirement, int resource)
    {
        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}

public class UserOwnsPartyRequirement : IAuthorizationRequirement { }
