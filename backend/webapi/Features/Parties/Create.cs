namespace Pidp.Features.Parties;

using FluentValidation;

using Pidp.Data;
using Pidp.Extensions;
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
            this.RuleFor(x => x.UserId).MatchesCurrentUser(accessor);
            this.RuleFor(x => x.FirstName).NotEmpty();
            this.RuleFor(x => x.LastName).NotEmpty();
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
