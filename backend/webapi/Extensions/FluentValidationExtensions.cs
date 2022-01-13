namespace Pidp.Extensions;

using FluentValidation;
using System.Security.Claims;

public static class FluentValidationExtensions
{
    /// <summary>
    /// Defines a validator on a rule builder for a Guid UserId property.
    /// Validation will fail if the property does not match the UserId of the current User.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <param name="httpContextAccessor"></param>
    public static IRuleBuilderOptionsConditions<T, Guid> MatchesCurrentUserId<T>(this IRuleBuilder<T, Guid> ruleBuilder, IHttpContextAccessor httpContextAccessor)
    {
        return ruleBuilder.Custom((userId, context) =>
        {
            if (userId != httpContextAccessor?.HttpContext?.User.GetUserId())
            {
                context.AddFailure("Must match the UserId of the current User");
            }
        });
    }

    /// <summary>
    /// Defines a validator on a rule builder for a Guid UserId.
    /// Validation will fail if the property does not match the value of the current User's specified Claim.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <param name="httpContextAccessor"></param>
    /// <param name="claimType"></param>
    public static IRuleBuilderOptionsConditions<T, string> MatchesCurrentUserClaim<T>(this IRuleBuilder<T, string> ruleBuilder, IHttpContextAccessor httpContextAccessor, string claimType)
    {
        return ruleBuilder.Custom((property, context) =>
        {
            if (property != httpContextAccessor?.HttpContext?.User?.FindFirstValue(claimType))
            {
                context.AddFailure($"Must match the \"{claimType}\" Claim on the current User");
            }
        });
    }
}
