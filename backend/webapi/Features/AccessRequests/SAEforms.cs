namespace Pidp.Features.AccessRequests;

using DomainResults.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NodaTime;

using Pidp.Data;
using Pidp.Extensions;
using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Infrastructure.HttpClients.Mail;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Infrastructure.Services;
using Pidp.Models;
using Pidp.Models.Lookups;

public class SAEforms
{
    public static IdentifierType[] ExcludedIdentifierTypes => [IdentifierType.PharmacyTech];

    public class Command : ICommand<IDomainResult>
    {
        public int PartyId { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator() => this.RuleFor(x => x.PartyId).GreaterThan(0);
    }

    public class CommandHandler(
        IClock clock,
        IEmailService emailService,
        IKeycloakAdministrationClient keycloakClient,
        ILogger<CommandHandler> logger,
        IPlrClient plrClient,
        PidpDbContext context) : ICommandHandler<Command, IDomainResult>
    {
        private readonly IClock clock = clock;
        private readonly IEmailService emailService = emailService;
        private readonly IKeycloakAdministrationClient keycloakClient = keycloakClient;
        private readonly ILogger<CommandHandler> logger = logger;
        private readonly IPlrClient plrClient = plrClient;
        private readonly PidpDbContext context = context;

        public async Task<IDomainResult> HandleAsync(Command command)
        {
            var dto = await this.context.Parties
                .Where(party => party.Id == command.PartyId)
                .Select(party => new
                {
                    AlreadyEnroled = party.AccessRequests.Any(request => request.AccessTypeCode == AccessTypeCode.SAEforms),
                    UserId = party.PrimaryUserId,
                    party.Email,
                    party.DisplayFirstName,
                    party.Cpn,
                })
                .SingleAsync();

            if (dto.AlreadyEnroled
                || dto.Email == null)
            {
                this.logger.LogAccessRequestDenied();
                return DomainResult.Failed();
            }

            if (dto.Cpn == null)
            {
                // Check status of Endorsements
                var endorsementCpns = await this.context.ActiveEndorsementRelationships(command.PartyId)
                    .Select(relationship => relationship.Party!.Cpn)
                    .ToListAsync();

                var endorsementPlrStanding = await this.plrClient.GetAggregateStandingsDigestAsync(endorsementCpns);
                if (!endorsementPlrStanding.With(ProviderRoleType.MedicalDoctor).HasGoodStanding)
                {
                    this.logger.LogAccessRequestDenied();
                    return DomainResult.Failed();
                }
            }
            else
            {
                if (!(await this.plrClient.GetStandingsDigestAsync(dto.Cpn))
                    .Excluding(ExcludedIdentifierTypes)
                    .HasGoodStanding)
                {
                    this.logger.LogAccessRequestDenied();
                    return DomainResult.Failed();
                }
            }

            if (!await this.keycloakClient.AssignAccessRoles(dto.UserId, MohKeycloakEnrolment.SAEforms))
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

            await this.SendConfirmationEmailAsync(dto.Email, dto.DisplayFirstName);

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
    public static partial void LogAccessRequestDenied(this ILogger<SAEforms.CommandHandler> logger);
}
