namespace Pidp.Features.AccessRequests;

using DomainResults.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NodaTime;

using Pidp.Data;
// using Pidp.Infrastructure.Auth;
// using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Infrastructure.HttpClients.Mail;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Infrastructure.Services;
using Pidp.Models;
using Pidp.Models.Lookups;

public class MSTeams
{
    public static IdentifierType[] AllowedIdentifierTypes => new[] { IdentifierType.PhysiciansAndSurgeons, IdentifierType.Nurse };

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
        // private readonly IKeycloakAdministrationClient keycloakClient;
        private readonly ILogger logger;
        private readonly IPlrClient plrClient;
        private readonly PidpDbContext context;

        public CommandHandler(
            IClock clock,
            IEmailService emailService,
            // IKeycloakAdministrationClient keycloakClient,
            ILogger<CommandHandler> logger,
            IPlrClient plrClient,
            PidpDbContext context)
        {
            this.clock = clock;
            this.emailService = emailService;
            // this.keycloakClient = keycloakClient;
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
                    AlreadyEnroled = party.AccessRequests.Any(request => request.AccessTypeCode == AccessTypeCode.MSTeams),
                    party.UserId,
                    party.Email,
                    party.Cpn,
                    Name = $"{party.FirstName} {party.LastName}"
                })
                .SingleAsync();

            if (dto.AlreadyEnroled
                || dto.Email == null
                || !(await this.plrClient.GetStandingsDigestAsync(dto.Cpn))
                    .With(AllowedIdentifierTypes)
                    .HasGoodStanding)
            {
                this.logger.LogMSTeamsAccessRequestDenied();
                return DomainResult.Failed();
            }

            // TODO Assign role / other enrolment actions
            // if (!await this.keycloakClient.AssignClientRole(dto.UserId, ?, ?))
            // {
            //     return DomainResult.Failed();
            // }

            this.context.AccessRequests.Add(new AccessRequest
            {
                PartyId = command.PartyId,
                AccessTypeCode = AccessTypeCode.MSTeams,
                RequestedOn = this.clock.GetCurrentInstant()
            });

            await this.context.SaveChangesAsync();

            await this.SendConfirmationEmailAsync(dto.Email, dto.Name);

            return DomainResult.Success();
        }

        private async Task SendConfirmationEmailAsync(string partyEmail, string partyName)
        {
            var email = new Email(
                from: EmailService.PidpEmail,
                to: partyEmail,
                subject: "MS Teams for Clinical Use Enrolment Confirmation",
                body: $"Dear {partyName},<br><br>You have been successfully enrolled for MS Teams for Clinical Use.<br>The Fraser Health mHealth team will contact you via email with account information and setup instructions. In the meantime please email <a href=\"mailto:securemessaging@fraserhealth.ca\">securemessaging@fraserhealth.ca</a> if you have any questions."
            );
            await this.emailService.SendAsync(email);
        }
    }
}

public static partial class MSTeamsLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Warning, "MS Teams Access Request denied due to the Party Record not meeting all prerequisites.")]
    public static partial void LogMSTeamsAccessRequestDenied(this ILogger logger);
}
