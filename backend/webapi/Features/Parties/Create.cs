namespace Pidp.Features.Parties;

using FluentValidation;

using Pidp.Data;
using Pidp.Extensions;
using Pidp.Infrastructure.Auth;
using Pidp.Models;

public class Create
{
    public class Command : ICommand<int>
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator(IHttpContextAccessor accessor)
        {
            this.RuleFor(x => x.UserId).NotEmpty().MatchesCurrentUserId(accessor);
            this.RuleFor(x => x.FirstName).NotEmpty().MatchesCurrentUserClaim(accessor, Claims.GivenName);
            this.RuleFor(x => x.LastName).NotEmpty().MatchesCurrentUserClaim(accessor, Claims.FamilyName);
        }
    }

    public class CommandHandler : ICommandHandler<Command, int>
    {
        private readonly PidpDbContext context;

        public CommandHandler(PidpDbContext context) => this.context = context;

        public async Task<int> HandleAsync(Command command)
        {
            var party = new Party
            {
                UserId = command.UserId,
                FirstName = command.FirstName,
                LastName = command.LastName,
            };

            this.context.Parties.Add(party);

            await this.context.SaveChangesAsync();

            return party.Id;
        }
    }
}
