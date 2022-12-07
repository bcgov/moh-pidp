namespace Pidp.Features.AccessRequests;

using DomainResults.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NodaTime;

using Pidp.Data;
using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Infrastructure.HttpClients.Mail;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Infrastructure.Services;
using Pidp.Models;
using Pidp.Models.Lookups;

public class PrescriptionRefillEforms
{
    public static IdentifierType[] AllowedIdentifierTypes => new[] { IdentifierType.Pharmacist };

    public class Command : ICommand<IDomainResult>
    {
        public int PartyId { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator() => this.RuleFor(x => x.PartyId).GreaterThan(0);
    }

    public class CommandHandler : ICommandHandler<Command, IDomainResult>
    {
        private readonly IClock clock;
        private readonly IEmailService emailService;
        private readonly IKeycloakAdministrationClient keycloakClient;
        private readonly ILogger logger;
        private readonly IPlrClient plrClient;
        private readonly PidpDbContext context;

        public CommandHandler(
            IClock clock,
            IEmailService emailService,
            IKeycloakAdministrationClient keycloakClient,
            ILogger<CommandHandler> logger,
            IPlrClient plrClient,
            PidpDbContext context)
        {
            this.clock = clock;
            this.emailService = emailService;
            this.keycloakClient = keycloakClient;
            this.logger = logger;
            this.plrClient = plrClient;
            this.context = context;
        }

        public async Task<IDomainResult> HandleAsync(Command command)
        {
            var userId = await this.context.Credentials
                .Where(credential => credential.PartyId == command.PartyId)
                .Select(credential => credential.UserId)
                .SingleAsync();

            var dto = await this.context.Parties
                .Where(party => party.Id == command.PartyId)
                .Select(party => new
                {
                    AlreadyEnroled = party.AccessRequests.Any(request => request.AccessTypeCode == AccessTypeCode.PrescriptionRefillEforms),
                    UserId = party.PrimaryUserId,
                    party.Email,
                    party.FirstName,
                    party.Cpn,
                })
                .SingleAsync();

            if (dto.AlreadyEnroled
                || dto.Email == null
                || !(await this.plrClient.GetStandingsDigestAsync(dto.Cpn))
                    .With(AllowedIdentifierTypes)
                    .HasGoodStanding)
            {
                this.logger.LogPrescriptionRefillEformsAccessRequestDenied();
                return DomainResult.Failed();
            }

            if (!await this.keycloakClient.AssignClientRole(dto.userId, MohClients.PrescriptionRefillEforms.ClientId, MohClients.PrescriptionRefillEforms.AccessRole))
            {
                return DomainResult.Failed();
            }

            this.context.AccessRequests.Add(new AccessRequest
            {
                PartyId = command.PartyId,
                AccessTypeCode = AccessTypeCode.PrescriptionRefillEforms,
                RequestedOn = this.clock.GetCurrentInstant()
            });

            await this.context.SaveChangesAsync();

            await this.SendConfirmationEmailAsync(dto.Email, dto.FirstName);

            return DomainResult.Success();
        }

        private async Task SendConfirmationEmailAsync(string partyEmail, string firstName)
        {
            var link = $"<a href=\"https://www.eforms.healthbc.org/login\" target=\"_blank\" rel=\"noopener noreferrer\">link</a>";
            var email = new Email(
                from: EmailService.PidpEmail,
                to: partyEmail,
                subject: "Prescription Refill eForms Enrolment Confirmation",
                body: $"Hi {firstName},<br><br>You will need to visit this {link} each time you want to submit an eForm. It may be helpful to bookmark this {link} for future use."
            );
            await this.emailService.SendAsync(email);
        }
    }
}

public static partial class PrescriptionRefillEformsLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Warning, "Prescription Refill eForm for Pharmacists denied due to the Party Record not meeting all prerequisites.")]
    public static partial void LogPrescriptionRefillEformsAccessRequestDenied(this ILogger logger);
}
