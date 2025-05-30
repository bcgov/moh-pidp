namespace Pidp.Features.EndorsementRequests;

using FluentValidation;
using Flurl;
using HybridModelBinding;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using System.Text.Json.Serialization;

using Pidp.Data;
using Pidp.Infrastructure.HttpClients.Mail;
using Pidp.Infrastructure.Services;
using Pidp.Models;

public class Create
{
    public class Command : ICommand<Model>
    {
        [JsonIgnore]
        [HybridBindProperty(Source.Route)]
        public int PartyId { get; set; }
        public string RecipientEmail { get; set; } = string.Empty;
        public string? AdditionalInformation { get; set; }
        public bool PreApproved { get; set; }
    }

    public class Model
    {
        public bool DuplicateEndorsementRequest { get; set; }

        public Model() { }

        public Model(bool duplicateEndorsementRequest) => this.DuplicateEndorsementRequest = duplicateEndorsementRequest;
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            this.RuleFor(x => x.PartyId).GreaterThan(0);
            this.RuleFor(x => x.RecipientEmail).NotEmpty().EmailAddress();
        }
    }

    public class CommandHandler(
        IClock clock,
        IEmailService emailService,
        PidpConfiguration config,
        PidpDbContext context) : ICommandHandler<Command, Model>
    {
        private readonly string applicationUrl = config.ApplicationUrl;
        private readonly IClock clock = clock;
        private readonly IEmailService emailService = emailService;
        private readonly PidpDbContext context = context;

        public async Task<Model> HandleAsync(Command command)
        {
            var isAlreadyEndorsed = await this.context.EndorsementRequests
                .AnyAsync(request => request.RequestingPartyId == command.PartyId
                    && request.RecipientEmail == command.RecipientEmail
                    && request.Status == EndorsementRequestStatus.Completed
                    && this.context.Endorsements
                        .Any(endorsement => endorsement.Active
                            && endorsement.EndorsementRelationships.Any(relation => relation.PartyId == request.RequestingPartyId)
                            && endorsement.EndorsementRelationships.Any(relation => relation.PartyId == request.ReceivingPartyId)));
            if (isAlreadyEndorsed)
            {
                return new(true);
            }

            var request = new EndorsementRequest
            {
                RequestingPartyId = command.PartyId,
                Token = Guid.NewGuid(),
                RecipientEmail = command.RecipientEmail,
                AdditionalInformation = command.AdditionalInformation,
                Status = EndorsementRequestStatus.Created,
                StatusDate = this.clock.GetCurrentInstant()
            };

            if (command.PreApproved)
            {
                var possibleRecipients = await this.context.Parties
                    .Where(party => party.Email!.ToLower() == command.RecipientEmail.ToLower())
                    .Select(party => party.Id)
                    .ToListAsync();

                if (possibleRecipients.Count == 1)
                {
                    request.ReceivingPartyId = possibleRecipients.Single();
                    request.Status = EndorsementRequestStatus.Received;
                    request.PreApproved = true;
                }
            }

            this.context.EndorsementRequests.Add(request);
            await this.context.SaveChangesAsync();

            var requestingPartyName = await this.context.Parties
                .Where(party => party.Id == command.PartyId)
                .Select(party => party.FullName)
                .SingleAsync();
            await this.SendEndorsementRequestEmailAsync(request.RecipientEmail, request.PreApproved ? null : request.Token, requestingPartyName);

            return new(false);
        }

        private async Task SendEndorsementRequestEmailAsync(string recipientEmail, Guid? token, string partyName)
        {
            string url = this.applicationUrl.SetQueryParam("endorsement-token", token);
            var link = $"<a href=\"{url}\" target=\"_blank\" rel=\"noopener noreferrer\">this link</a>";
            var pidpSupportEmail = $"<a href=\"mailto:{EmailService.PidpEmail}\">{EmailService.PidpEmail}</a>";
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
