namespace Pidp.Features.EndorsementRequests;

using FluentValidation;
using HybridModelBinding;
using System.Text.Json.Serialization;

using Pidp.Data;
using Pidp.Infrastructure.Services;
using Pidp.Infrastructure.HttpClients.Mail;
using Pidp.Models;

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
        private readonly IEmailService emailService;
        private readonly PidpDbContext context;

        public CommandHandler(IEmailService emailService, PidpDbContext context)
        {
            this.emailService = emailService;
            this.context = context;
        }

        public async Task HandleAsync(Command command)
        {
            var request = new EndorsementRequest
            {
                RequestingPartyId = command.PartyId,
                Token = Guid.NewGuid(),
                RecipientEmail = command.RecipientEmail,
                JobTitle = command.JobTitle
            };

            this.context.EndorsementRequests.Add(request);
            await this.context.SaveChangesAsync();

            await this.SendEndorsementRequestEmailAsync(request.RecipientEmail, request.Token);
        }

        private async Task SendEndorsementRequestEmailAsync(string recipientEmail, Guid token)
        {
            // var link = $"<a href=\"https://www.eforms.healthbc.org/login?sat=true\" target=\"_blank\" rel=\"noopener noreferrer\">link</a>";
            var email = new Email(
                from: EmailService.PidpEmail,
                to: recipientEmail,
                subject: "You Have Recieved an Endorement Request in PIdP",
                body: $"Click: {token}."
            );
            await this.emailService.SendAsync(email);
        }
    }
}
