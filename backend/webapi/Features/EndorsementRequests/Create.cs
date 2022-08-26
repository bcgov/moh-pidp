namespace Pidp.Features.EndorsementRequests;

using FluentValidation;
using Flurl;
using HybridModelBinding;
using System.Text.Json.Serialization;

using Pidp.Data;
using Pidp.Infrastructure.Services;
using Pidp.Infrastructure.HttpClients.Mail;
using Pidp.Models;
using NodaTime;

public class Create
{
    public class Command : ICommand
    {
        [JsonIgnore]
        [HybridBindProperty(Source.Route)]
        public int PartyId { get; set; }
        public string RecipientEmail { get; set; } = string.Empty;
        public string? AdditionalInformation { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            this.RuleFor(x => x.PartyId).GreaterThan(0);
            this.RuleFor(x => x.RecipientEmail).NotEmpty().EmailAddress();
        }
    }

    public class CommandHandler : ICommandHandler<Command>
    {
        private readonly string applicationUrl;
        private readonly IClock clock;
        private readonly IEmailService emailService;
        private readonly PidpDbContext context;

        public CommandHandler(
            IClock clock,
            IEmailService emailService,
            PidpConfiguration config,
            PidpDbContext context)
        {
            this.applicationUrl = config.ApplicationUrl;
            this.clock = clock;
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
                AdditionalInformation = command.AdditionalInformation,
                Status = EndorsementRequestStatus.Created,
                StatusDate = this.clock.GetCurrentInstant()
            };

            this.context.EndorsementRequests.Add(request);
            await this.context.SaveChangesAsync();

            await this.SendEndorsementRequestEmailAsync(request.RecipientEmail, request.Token);
        }

        private async Task SendEndorsementRequestEmailAsync(string recipientEmail, Guid token)
        {
            string url = this.applicationUrl.SetQueryParam("endorsement-token", token);
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
