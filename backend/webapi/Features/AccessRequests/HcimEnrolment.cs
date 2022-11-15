namespace Pidp.Features.AccessRequests;

using DomainResults.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NodaTime;

using Pidp.Data;
// using Pidp.Infrastructure.Auth;
// using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Infrastructure.HttpClients.Mail;
using Pidp.Infrastructure.Services;
using Pidp.Models.Lookups;

public class HcimEnrolment
{
    public class Command : ICommand<IDomainResult>
    {
        public int PartyId { get; set; }
        public bool ManagesTasks { get; set; }
        public bool ModifiesPhns { get; set; }
        public bool RecordsNewborns { get; set; }
        public bool SearchesIdentifiers { get; set; }
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
        private readonly PidpDbContext context;

        public CommandHandler(
            IClock clock,
            IEmailService emailService,
            // IKeycloakAdministrationClient keycloakClient,
            ILogger<CommandHandler> logger,
            PidpDbContext context)
        {
            this.clock = clock;
            this.emailService = emailService;
            // this.keycloakClient = keycloakClient;
            this.logger = logger;
            this.context = context;
        }

        public async Task<IDomainResult> HandleAsync(Command command)
        {
            var dto = await this.context.Parties
                .Where(party => party.Id == command.PartyId)
                .Select(party => new
                {
                    AlreadyEnroled = party.AccessRequests.Any(request => request.AccessTypeCode == AccessTypeCode.HcimAccountTransfer
                        || request.AccessTypeCode == AccessTypeCode.HcimEnrolment),
                    DemographicsComplete = party.Email != null && party.Phone != null,
                    AdminEmail = party.AccessAdministrator!.Email,
                })
                .SingleAsync();

            if (dto.AlreadyEnroled
                || !dto.DemographicsComplete
                || dto.AdminEmail == null)
            {
                this.logger.LogHcimEnrolmentAccessRequestDenied();
                return DomainResult.Failed();
            }

            // TODO assign role?
            // if (!await this.keycloakClient.AssignClientRole(dto.UserId, ?, ?))
            // {
            //     return DomainResult.Failed();
            // }

            this.context.HcimEnrolments.Add(new Models.HcimEnrolment
            {
                PartyId = command.PartyId,
                AccessTypeCode = AccessTypeCode.HcimEnrolment,
                RequestedOn = this.clock.GetCurrentInstant(),
                ManagesTasks = command.ManagesTasks,
                ModifiesPhns = command.ModifiesPhns,
                RecordsNewborns = command.RecordsNewborns,
                SearchesIdentifiers = command.SearchesIdentifiers,
            });

            await this.context.SaveChangesAsync();

            await this.SendConfirmationEmailAsync(dto.AdminEmail);

            return DomainResult.Success();
        }

        private async Task SendConfirmationEmailAsync(string adminEmail)
        {
            // TODO email text
            // var link = $"<a href=\"https://www.eforms.healthbc.org/login\" target=\"_blank\" rel=\"noopener noreferrer\">link</a>";
            var email = new Email(
                from: EmailService.PidpEmail,
                to: adminEmail,
                subject: "HCIMWeb Enrolment Confirmation",
                body: $"Placeholder Email."
            );
            await this.emailService.SendAsync(email);
        }
    }
}

public static partial class HcimEnrolmentLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Warning, "HCIM Enrolment Access Request denied due to the Party Record not meeting all prerequisites.")]
    public static partial void LogHcimEnrolmentAccessRequestDenied(this ILogger logger);
}
