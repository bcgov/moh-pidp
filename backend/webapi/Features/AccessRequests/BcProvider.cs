namespace Pidp.Features.AccessRequests;

using DomainResults.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NodaTime;

using System.Text.RegularExpressions;

using Pidp.Data;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models;
using Pidp.Models.Lookups;

public class BcProvider
{
    public static IdentifierType[] AllowedIdentifierTypes => new[] { IdentifierType.PhysiciansAndSurgeons };
    /// <summary>
    /// Adapted from https://uibakery.io/regex-library/password-regex-csharp
    /// </summary>
    private static readonly string PASSWORD_REGEX = "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,32}$";

    public class Command : ICommand<IDomainResult>
    {
        public int PartyId { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            this.RuleFor(x => x.PartyId).GreaterThan(0);
            this.RuleFor(x => x.Username).NotEmpty().EmailAddress()
                .WithMessage("A BC Provider account was not created. Username is not in the correct format.");
            this.RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(32)
                .Matches(PASSWORD_REGEX, RegexOptions.Singleline)
                .WithMessage("A BC Provider account was not created. Please check that your password meets all password rules.");
        }
    }

    public class CommandHandler : ICommandHandler<Command, IDomainResult>
    {
        public Task<IDomainResult> HandleAsync(Command command)
        {
            if (command.Password?.Contains("servererror", StringComparison.OrdinalIgnoreCase) == true)
            {
                throw new Exception("TEST EXCEPTION");
            }
            return Task.FromResult(DomainResult.Success());
        }
    }
}
