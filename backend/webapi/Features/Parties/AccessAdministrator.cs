namespace Pidp.Features.Parties;

using FluentValidation;
using HybridModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

using Pidp.Data;

public class AccessAdministrator
{
    public class Query : IQuery<Command>
    {
        public int PartyId { get; set; }
    }

    public class Command : ICommand
    {
        [JsonIgnore]
        [HybridBindProperty(Source.Route)]
        public int PartyId { get; set; }

        public string Email { get; set; } = string.Empty;
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator() => this.RuleFor(x => x.PartyId).GreaterThan(0);
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            this.RuleFor(x => x.PartyId).GreaterThan(0);
            this.RuleFor(x => x.Email).NotEmpty().EmailAddress(); // TODO Custom email validation?
        }
    }

    public class QueryHandler : IQueryHandler<Query, Command>
    {
        private readonly PidpDbContext context;

        public QueryHandler(PidpDbContext context) => this.context = context;

        public async Task<Command> HandleAsync(Query query)
        {
            var admin = await this.context.AccessAdministrators
                .Where(admin => admin.PartyId == query.PartyId)
                .Select(admin => new Command
                {
                    PartyId = admin.PartyId,
                    Email = admin.Email
                })
                .SingleOrDefaultAsync();

            return admin ?? new Command { PartyId = query.PartyId };
        }
    }

    public class CommandHandler : ICommandHandler<Command>
    {
        private readonly PidpDbContext context;

        public CommandHandler(PidpDbContext context) => this.context = context;

        public async Task HandleAsync(Command command)
        {
            var admin = await this.context.AccessAdministrators
                .SingleOrDefaultAsync(admin => admin.PartyId == command.PartyId);

            if (admin == null)
            {
                admin = new Models.AccessAdministrator
                {
                    PartyId = command.PartyId
                };
                this.context.AccessAdministrators.Add(admin);
            }

            admin.Email = command.Email;

            await this.context.SaveChangesAsync();
        }
    }
}
