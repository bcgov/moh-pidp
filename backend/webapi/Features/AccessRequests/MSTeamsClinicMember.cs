namespace Pidp.Features.AccessRequests;

using AutoMapper;
using DomainResults.Common;
using FluentValidation;
using HybridModelBinding;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using System.Text.Json;
using System.Text.Json.Serialization;

using Pidp.Data;
using Pidp.Extensions;
using Pidp.Infrastructure.HttpClients.Mail;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Infrastructure.Services;
using Pidp.Models;
using Pidp.Models.Lookups;

public class MSTeamsClinicMember
{
    public class Command : ICommand<IDomainResult>
    {
        [JsonIgnore]
        [HybridBindProperty(Source.Route)]
        public int PartyId { get; set; }
        public int ClinicId { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            this.RuleFor(x => x.PartyId).GreaterThan(0);
            this.RuleFor(x => x.ClinicId).GreaterThan(0);
        }
    }

    public class CommandHandler : ICommandHandler<Command, IDomainResult>
    {
        private readonly IClock clock;
        private readonly IEmailService emailService;
        private readonly ILogger logger;
        private readonly IMapper mapper;
        private readonly IPlrClient plrClient;
        private readonly PidpDbContext context;

        public CommandHandler(
            IClock clock,
            IEmailService emailService,
            ILogger<CommandHandler> logger,
            IMapper mapper,
            IPlrClient plrClient,
            PidpDbContext context)
        {
            this.clock = clock;
            this.emailService = emailService;
            this.logger = logger;
            this.mapper = mapper;
            this.plrClient = plrClient;
            this.context = context;
        }

        public async Task<IDomainResult> HandleAsync(Command command)
        {
            var dto = await this.context.Parties
                .Where(party => party.Id == command.PartyId)
                .Select(party => new
                {
                    AlreadyEnroled = party.AccessRequests.Any(request => request.AccessTypeCode == AccessTypeCode.MSTeamsClinicMember),
                    party.Email,
                    party.FirstName,
                })
                .SingleAsync();

            var validClinic = await this.context.MSTeamsClinics.AnyAsync(clinic => clinic.Id == command.ClinicId
                && this.context.ActiveEndorsementRelationships(command.PartyId)
                    .Any(relationship => relationship.PartyId == clinic.PrivacyOfficerId));

            if (dto.AlreadyEnroled
                || dto.Email == null
                || !validClinic)
            {
                this.logger.LogMSTeamsClinicMemberAccessRequestDenied();
                return DomainResult.Failed();
            }

            this.context.MSTeamsClinicMemberEnrolments.Add(new MSTeamsClinicMemberEnrolment
            {
                PartyId = command.PartyId,
                AccessTypeCode = AccessTypeCode.MSTeamsClinicMember,
                RequestedOn = this.clock.GetCurrentInstant(),
                ClinicId = command.ClinicId
            });

            await this.context.SaveChangesAsync();

            return DomainResult.Success();
        }

        private async Task SendEnrolmentEmailAsync()
        {
            var email = new Email(
                from: EmailService.PidpEmail,
                to: "enrolment_securemessagingsupport@fraserhealth.ca",
                subject: "New MS Teams for Clinical Use Clinic Member Enrolment",
                body: $"<pre>{JsonSerializer.Serialize(new { }, new JsonSerializerOptions { WriteIndented = true })}</pre>"
            );
            await this.emailService.SendAsync(email);
        }

        private async Task SendConfirmationEmailAsync(string partyEmail, string partyName)
        {
            var email = new Email(
                from: EmailService.PidpEmail,
                to: partyEmail,
                subject: "MS Teams for Clinical Use Enrolment Confirmation",
                body: $"Dear {partyName},<br><br>You have been successfully enrolled for MS Teams for Clinical Use.<br>The Fraser Health mHealth team will contact you via email with account information and setup instructions. In the meantime please email <a href=\"mailto:securemessagingsupport@fraserhealth.ca\">securemessagingsupport@fraserhealth.ca</a> if you have any questions."
            );
            await this.emailService.SendAsync(email);
        }
    }
}

public static partial class MSTeamsClinicMemberLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Warning, "MS Teams Clinic Member Access Request denied due to the Party Record not meeting all prerequisites.")]
    public static partial void LogMSTeamsClinicMemberAccessRequestDenied(this ILogger logger);
}
