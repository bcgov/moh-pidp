namespace Pidp.Extensions;

using FluentValidation;
using System.Security.Claims;

public static class FluentValidationExtensions
{
    /// <summary>
    /// Defines a validator on a rule builder for a string property.
    /// Validation will fail if the property does not match the value of the current User's specified Claim, or if User is null.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ruleBuilder"></param>
    /// <param name="httpContextAccessor"></param>
    /// <param name="claimType"></param>
    public static IRuleBuilderOptionsConditions<T, string?> MatchesUserClaim<T>(this IRuleBuilder<T, string?> ruleBuilder, ClaimsPrincipal? user, string claimType)
    {
        return ruleBuilder.Custom((property, context) =>
        {
            if (user == null)
            {
                context.AddFailure("No User found");
                return;
            }

            if (property != user.FindFirstValue(claimType))
            {
                context.AddFailure($"Must match the \"{claimType}\" Claim on the current User");
            }
        });
    }

    /// <summary>
    /// Defines a validator on a Rule builder for a string property.
    /// Validation will fail if the property is not null and contains only whitespace characters.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ruleBuilder"></param>
    public static IRuleBuilderOptions<T, string?> NotWhiteSpace<T>(this IRuleBuilder<T, string?> ruleBuilder) => ruleBuilder.Must(value => value == null || !string.IsNullOrWhiteSpace(value));
}
