namespace Pidp.Features.Parties;

using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using Pidp.Data;

public partial class UserAccessAgreement
{
    public class Command : ICommand
    {
        public int Id { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator() => this.RuleFor(x => x.Id).GreaterThan(0);
    }

    public class CommandHandler : ICommandHandler<Command>
    {
        private readonly IClock clock;
        private readonly PidpDbContext context;

        public CommandHandler(IClock clock, PidpDbContext context)
        {
            this.clock = clock;
            this.context = context;
        }

        public async Task HandleAsync(Command command)
        {
            var party = await this.context.Parties
                .SingleAsync(party => party.Id == command.Id);

            if (party.UserAccessAgreementDate != null)
            {
                return;
            }

            party.UserAccessAgreementDate = this.clock.GetCurrentInstant();

            await this.context.SaveChangesAsync();
        }
    }
}
