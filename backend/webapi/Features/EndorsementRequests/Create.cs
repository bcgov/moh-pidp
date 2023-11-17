namespace Pidp.Features.EndorsementRequests;

using FluentValidation;
using Flurl;
using HybridModelBinding;
using NodaTime;
using System.Text.Json.Serialization;

using Pidp.Data;
using Pidp.Infrastructure.HttpClients.Mail;
using Pidp.Infrastructure.Services;
using Pidp.Models;
using Microsoft.EntityFrameworkCore;

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
            var partyName = await this.context.Parties
                .Where(party => party.Id == command.PartyId)
                .Select(party => party.FullName)
                .SingleAsync();

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

            await this.SendEndorsementRequestEmailAsync(request.RecipientEmail, request.Token, partyName);
        }

        private async Task SendEndorsementRequestEmailAsync(string recipientEmail, Guid token, string partyName)
        {
            string url = this.applicationUrl.SetQueryParam("endorsement-token", token);
            var link = $"<a href=\"{url}\" target=\"_blank\" rel=\"noopener noreferrer\">this link</a>";
            var pidpSupportEmail = $"<a href=\"mailto:{EmailService.PidpEmail}\">${EmailService.PidpEmail}</a>";
            var pidpSupportPhone = $"<a href=\"tel:{EmailService.PidpSupportPhone}\">{EmailService.PidpSupportPhone}</a>";

            var email = new Email(
                from: EmailService.PidpEmail,
                to: recipientEmail,
                subject: $"OneHealthID Endorsement Request from {partyName}",
                body: $@"Hello,
<br>You are receiving this email because a user requested an endorsement from you.
<br>
<br>To complete the endorsement process, use {link} to log into the OneHealthID Service with your BC Services Card.
<br>
<br>After logging in, please:
<br>&emsp;1. Complete the “Contact Information” tile.
<br>&emsp;2. Declare your college information (even if you do not have a college licence).
<br>&emsp;3. Click on the “Endorsements” tile in the “Organization Information” section, and follow the prompts.
<br>
<br>For additional support, contact the OneHealthID Service desk:
<br>
<br>&emsp;• By email at {pidpSupportEmail}
<br>
<br>&emsp;• By phone at {pidpSupportPhone}
<br>
<br>Thank you.");

            await this.emailService.SendAsync(email);
        }
    }
}
