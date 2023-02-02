namespace Pidp.Features.BCProvider;

using DomainResults.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Pidp.Infrastructure.HttpClients.Plr;

public class BCProviderChangePassword
{
    public static IdentifierType[] AllowedIdentifierTypes => new[] { IdentifierType.PhysiciansAndSurgeons };
    /// <summary>
    /// Adapted from https://uibakery.io/regex-library/password-regex-csharp
    /// </summary>
    public static readonly string PASSWORDREGEX = "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,32}$";

    public class Command : ICommand<IDomainResult>
    {
        public int PartyId { get; set; }
        public string? Username { get; set; }
        public string? CurrentPassword { get; set; }
        public string? NewPassword { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            this.RuleFor(x => x.PartyId).GreaterThan(0);
            this.RuleFor(x => x.Username).NotEmpty().EmailAddress()
                .WithMessage("A BC Provider account was not created. Username is not in the correct format.");
            this.RuleFor(x => x.CurrentPassword).NotEmpty();
            this.RuleFor(x => x.NewPassword)
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(32)
                .Matches(PASSWORDREGEX, RegexOptions.Singleline)
                .WithMessage("A BC Provider account was not created. Please check that your password meets all password rules.");
            this.RuleFor(x => x.NewPassword)
                .NotEqual(x => x.CurrentPassword)
                .WithMessage("New password must be different from the previous password.");
        }
    }

    public class CommandHandler : ICommandHandler<Command, IDomainResult>
    {
        public Task<IDomainResult> HandleAsync(Command command)
        {
            if (command.NewPassword?.Contains("servererror", StringComparison.OrdinalIgnoreCase) == true)
            {
#pragma warning disable CA2201 // Do not raise reserved exception types
                throw new Exception("TEST EXCEPTION");
#pragma warning restore CA2201 // Do not raise reserved exception types
            }
            return Task.FromResult(DomainResult.Success());
        }
    }
}
