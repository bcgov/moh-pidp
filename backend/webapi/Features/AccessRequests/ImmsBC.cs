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

public class ImmsBC
{
    public static IdentifierType[] AllowedIdentifierTypes => [IdentifierType.PhysiciansAndSurgeons, IdentifierType.Pharmacist, IdentifierType.PharmacyTech, IdentifierType.Nurse, IdentifierType.Midwife];
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
        private readonly IEmailService emailService;
        private readonly IClock clock;
        private readonly ILogger<CommandHandler> logger;
        private readonly IKeycloakAdministrationClient keycloakClient;
        private readonly PidpDbContext context;
        private readonly IPlrClient plrClient;

        public CommandHandler(
            IEmailService emailService,
            IClock clock,
            ILogger<CommandHandler> logger,
            IKeycloakAdministrationClient keycloakClient,
            PidpDbContext context,
            IPlrClient plrClient)
        {
            this.emailService = emailService;
            this.clock = clock;
            this.logger = logger;
            this.keycloakClient = keycloakClient;
            this.context = context;
            this.plrClient = plrClient;
        }

        public async Task<IDomainResult> HandleAsync(Command command)
        {
            var dataObj = await this.context.Parties
                .Where(party => party.Id == command.PartyId)
                .Select(party => new
                {
                    AlreadyEnroled = party.AccessRequests.Any(request => request.AccessTypeCode == AccessTypeCode.ImmsBC),
                    UserId = party.PrimaryUserId,
                    party.Email,
                    party.DisplayFirstName,
                    party.Cpn,
                })
                .SingleAsync();

            if (dataObj.AlreadyEnroled
                || dataObj.Email == null
                || !await this.plrClient.GetStandingAsync(dataObj.Cpn)
                || !(await this.plrClient.GetStandingsDigestAsync(dataObj.Cpn))
                    .With(AllowedIdentifierTypes).HasGoodStanding)
            {
                this.logger.LogAccessRequestDenied(command.PartyId);
                return DomainResult.Failed();
            }


            this.context.AccessRequests.Add(new AccessRequest
            {
                PartyId = command.PartyId,
                AccessTypeCode = AccessTypeCode.ImmsBC,
                RequestedOn = this.clock.GetCurrentInstant()
            });

            await this.context.SaveChangesAsync();

            await this.SendConfirmationEmailAsync(dataObj.Email, dataObj.DisplayFirstName);

            return DomainResult.Success();
        }

        private async Task SendConfirmationEmailAsync(string partyEmail, string firstName)
        {
            var link = $"<a href=\"https://www.eforms.healthbc.org/login\" target=\"_blank\" rel=\"noopener noreferrer\">link</a>";
            var email = new Email(
                from: EmailService.PidpEmail,
                to: partyEmail,
                subject: "ImmsBC Enrolment Confirmation",
                body: $"Hi {firstName},<br><br>You will need to visit this {link} each time you want to submit an eForm. It may be helpful to bookmark this {link} for future use."
            );
            await this.emailService.SendAsync(email);
        }
    }
}

public static partial class ImmsBCLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Warning, "ImmsBC Access Request for Party {partyId} denied; did not meet all prerequisites.")]
    public static partial void LogAccessRequestDenied(this ILogger<ImmsBC.CommandHandler> logger, int partyId);
}
