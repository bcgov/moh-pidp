namespace Pidp.Features.Parties;

using FluentValidation;
using NodaTime;

using Pidp.Data;
using Pidp.Models;

public class Create
{
    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            this.RuleFor(x => x.FirstName).NotEmpty();
            this.RuleFor(x => x.LastName).NotEmpty();
            this.RuleFor(x => x.DateOfBirth).NotEmpty();
        }
    }

    public class Command : ICommand<int>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public LocalDate DateOfBirth { get; set; }
    }

    public class CreateCommandHandler : ICommandHandler<Command, int>
    {
        private readonly PidpDbContext context;

        public CreateCommandHandler(PidpDbContext context) => this.context = context;

        public async Task<int> HandleAsync(Command command)
        {
            var party = new Party
            {
                FirstName = command.FirstName,
                LastName = command.LastName,
                DateOfBirth = command.DateOfBirth
            };

            this.context.Parties.Add(party);

            await this.context.SaveChangesAsync();

            return party.Id;
        }
    }
}
