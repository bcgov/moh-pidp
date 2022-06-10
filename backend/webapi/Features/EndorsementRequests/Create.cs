namespace Pidp.Features.EndorsementRequests;

using FluentValidation;

using Pidp.Data;
using Pidp.Models;
using HybridModelBinding;
using System.Text.Json.Serialization;

public class Create
{
    public class Command : ICommand
    {
        [JsonIgnore]
        [HybridBindProperty(Source.Route)]
        public int PartyId { get; set; }
        public string RecipientEmail { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            this.RuleFor(x => x.PartyId).GreaterThan(0);
            this.RuleFor(x => x.RecipientEmail).NotEmpty();
            // this.RuleFor(x => x.JobTitle).NotEmpty();
        }
    }

    public class CommandHandler : ICommandHandler<Command>
    {
        private readonly PidpDbContext context;

        public CommandHandler(PidpDbContext context) => this.context = context;

        public async Task HandleAsync(Command command)
        {
            this.context.EndorsementRequests.Add(new EndorsementRequest
            {
                RequestingPartyId = command.PartyId,
                Token = Guid.NewGuid(),
                RecipientEmail = command.RecipientEmail,
                JobTitle = command.JobTitle
            });

            await this.context.SaveChangesAsync();

            // TODO: generate and send email
        }
    }
}
