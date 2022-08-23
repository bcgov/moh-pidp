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

public class SAEforms
{
    public static IdentifierType[] ExcludedIdentifierTypes => new[] { IdentifierType.Pharmacist, IdentifierType.PharmacyTech };

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
            var dto = await this.context.Parties
                .Where(party => party.Id == command.PartyId)
                .Select(party => new
                {
                    AlreadyEnroled = party.AccessRequests.Any(request => request.AccessTypeCode == AccessTypeCode.SAEforms),
                    party.UserId,
                    party.Email,
                    party.FirstName,
                    party.Cpn,
                })
                .SingleAsync();

            if (dto.AlreadyEnroled
                || dto.Email == null
                || !(await this.plrClient.GetStandingsDigestAsync(dto.Cpn))
                    .Excluding(ExcludedIdentifierTypes)
                    .HasGoodStanding)
            {
                this.logger.LogSAEformsAccessRequestDenied();
                return DomainResult.Failed();
            }

            if (!await this.keycloakClient.AssignClientRole(dto.UserId, MohClients.SAEforms.ClientId, MohClients.SAEforms.AccessRole))
            {
                return DomainResult.Failed();
            }

            this.context.AccessRequests.Add(new AccessRequest
            {
                PartyId = command.PartyId,
                AccessTypeCode = AccessTypeCode.SAEforms,
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
                subject: "SA eForms Enrolment Confirmation",
                body: $"Hi {firstName},<br><br>You will need to visit this {link} each time you want to submit an SA eForm. It may be helpful to bookmark this {link} for future use."
            );
            await this.emailService.SendAsync(email);
        }
    }
}

public static partial class SAEformsLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Warning, "SA eForms Access Request denied due to the Party Record not meeting all prerequisites.")]
    public static partial void LogSAEformsAccessRequestDenied(this ILogger logger);
}
