namespace Pidp.Features.EndorsementRequests;

using FluentValidation;
using Flurl;
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
            this.RuleFor(x => x.RecipientEmail).NotEmpty().EmailAddress();
            this.RuleFor(x => x.JobTitle).NotEmpty();
        }
    }

    public class CommandHandler : ICommandHandler<Command>
    {
        private readonly IEmailService emailService;
        private readonly PidpDbContext context;
        private readonly string frontendUrl;

        public CommandHandler(
            IEmailService emailService,
            PidpConfiguration config,
            PidpDbContext context)
        {
            this.emailService = emailService;
            this.context = context;
            this.frontendUrl = config.ApplicationUrl;
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
            string url = this.frontendUrl.SetQueryParam("endorsement-token", token);
            var link = $"<a href=\"{url}\" target=\"_blank\" rel=\"noopener noreferrer\">this link</a>";
            var email = new Email(
                from: EmailService.PidpEmail,
                to: recipientEmail,
                subject: "You Have Received an Endorsement Request in PIdP",
                body: $"You have a new endorsement in the Provider Identity Portal. Please use {link} and log in to the Provider Identity Portal to complete your enrolment(s)"
            );
            await this.emailService.SendAsync(email);
        }
    }
}
