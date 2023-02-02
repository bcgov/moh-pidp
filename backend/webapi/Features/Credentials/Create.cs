namespace Pidp.Features.Credentials;

using FluentValidation;
using System.Text.RegularExpressions;

using Pidp.Infrastructure.Auth;
using Pidp.Features.BCProvider;

public class Create
{
    public class Command : ICommand<int>
    {
        public int PartyId { get; set; }
        public string IdentityProvider { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator(IHttpContextAccessor accessor)
        {
            var user = accessor?.HttpContext?.User;

            this.RuleFor(x => x.PartyId).GreaterThan(0);
            this.RuleFor(x => x.IdentityProvider).NotEmpty()
                .Equal(IdentityProviders.BCProvider).WithMessage("Only BC Provider is supported at this time");
            this.RuleFor(x => x.Username).NotEmpty();
            this.RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(32)
                .Matches(BCProviderChangePassword.PASSWORDREGEX, RegexOptions.Singleline)
                .WithMessage("A BC Provider account was not created. Please check that your password meets all password rules.");
        }
    }

    public class CommandHandler : ICommandHandler<Command, int>
    {
        public Task<int> HandleAsync(Command command)
            => Task.FromResult(1);
    }
}
