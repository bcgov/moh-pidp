namespace Pidp.Extensions;

using FluentValidation;

public static class FluentValidationExtensions
{
    /// <summary>
    /// Defines a validator on a rule builder for a Guid UserId.
    /// Validation will fail if the UserId is Guid.Empty or if it does not match the 'sub' field of the current User's claims.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <param name="httpContextAccessor"></param>
    public static IRuleBuilderOptionsConditions<T, Guid> MatchesCurrentUser<T>(this IRuleBuilder<T, Guid> ruleBuilder, IHttpContextAccessor httpContextAccessor)
    {
        return ruleBuilder.NotEmpty().Custom((userId, context) =>
        {
            if (userId != httpContextAccessor?.HttpContext?.User.GetUserId())
            {
                context.AddFailure("UserId must match the subject of the Auth Token");
            }
        });
    }
}
