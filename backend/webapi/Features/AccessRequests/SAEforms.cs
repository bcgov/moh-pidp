namespace Pidp.Features.AccessRequests;

using DomainResults.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NodaTime;

using Pidp.Data;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Infrastructure.HttpClients.Mail;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Infrastructure.Services;
using Pidp.Models;
using Pidp.Models.Lookups;

public class SAEforms
{
    public static IdentifierType[] ExcludedIdentifierTypes => [IdentifierType.PharmacyTech];

    public static bool IsEligible(PlrStandingsDigest partyPlrStanding)
    {
        return partyPlrStanding
            .Excluding(ExcludedIdentifierTypes)
            .HasGoodStanding || partyPlrStanding.IsCpsPostgrad;
    }

    public static async Task<bool> GrantAsync(int partyId, IEnumerable<Guid> userIds, IKeycloakAdministrationClient keycloakClient, PidpDbContext context, IClock clock)
    {
        foreach (var userId in userIds)
        {
            if (!await keycloakClient.AssignAccessRoles(userId, MohKeycloakEnrolment.SAEforms))
            {
                return false;
            }
        }

        context.AccessRequests.Add(new AccessRequest
        {
            PartyId = partyId,
            AccessTypeCode = AccessTypeCode.SAEforms,
            RequestedOn = clock.GetCurrentInstant()
        });

        context.BusinessEvents.Add(AccessRequestSubmitted.Create(partyId, AccessTypeCode.SAEforms.ToString(), clock.GetCurrentInstant()));

        await context.SaveChangesAsync();

        return true;
    }

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
                    AlreadyEnroledInNpdp = party.AccessRequests.Any(request => request.AccessTypeCode == AccessTypeCode.NpdpEforms),
                    UserIds = party.Credentials
                        .Where(credential => credential.IdentityProvider == IdentityProviders.BCServicesCard || credential.IdentityProvider == IdentityProviders.BCProvider)
                        .Select(credential => credential.UserId),
                    // NPDP eForms is granted to BC Services Card credentials only
                    NpdpUserIds = party.Credentials
                        .Where(credential => credential.IdentityProvider == IdentityProviders.BCServicesCard)
                        .Select(credential => credential.UserId),
                    party.Email,
                    party.DisplayFirstName,
                    party.DisplayLastName,
                    party.Cpn,
                })
                .SingleAsync();

            var plrStanding = await this.plrClient.GetStandingsDigestAsync(dto.Cpn);

            if (dto.AlreadyEnroled
                || dto.Email == null
                || !IsEligible(plrStanding))
            {
                this.logger.LogAccessRequestDenied();
                this.context.BusinessEvents.Add(AccessRequestFailed.Create(command.PartyId, AccessTypeCode.SAEforms.ToString(), this.clock.GetCurrentInstant()));
                await this.context.SaveChangesAsync();
                return DomainResult.Failed();
            }

            if (!await GrantAsync(command.PartyId, dto.UserIds, this.keycloakClient, this.context, this.clock))
            {
                return DomainResult.Failed();
            }

            var recipientName = dto.DisplayFirstName ?? dto.DisplayLastName;

            await this.SendConfirmationEmailAsync(dto.Email, recipientName);

            // Pharmacists that apply for SAEforms should get NPDP access
            if (!dto.AlreadyEnroledInNpdp
                && NpdpEforms.IsEligible(plrStanding)
                && dto.NpdpUserIds.Any()
                && !await NpdpEforms.GrantAsync(command.PartyId, dto.NpdpUserIds, this.keycloakClient, this.context, this.clock))
            {
                this.logger.LogPairedNpdpGrantFailed(command.PartyId);
            }

            return DomainResult.Success();
        }

        private async Task SendConfirmationEmailAsync(string partyEmail, string recipientName)
        {
            var link = $"<a href=\"https://www.eforms.healthbc.org/login\" target=\"_blank\" rel=\"noopener noreferrer\">link</a>";
            var email = new Email(
                from: EmailService.PidpEmail,
                to: partyEmail,
                subject: "SA eForms Enrolment Confirmation",
                body: $"Hi {recipientName},<br><br>You will need to visit this {link} each time you want to submit an SA eForm. It may be helpful to bookmark this {link} for future use."
            );
            await this.emailService.SendAsync(email);
        }
    }
}

public static partial class SAEformsLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Warning, "SA eForms Access Request denied due to the Party Record not meeting all prerequisites.")]
    public static partial void LogAccessRequestDenied(this ILogger<SAEforms.CommandHandler> logger);

    [LoggerMessage(2, LogLevel.Warning, "NPDP eForms could not be granted to Party {partyId} alongside their SA eForms enrolment; the SA eForms access itself was not affected.")]
    public static partial void LogPairedNpdpGrantFailed(this ILogger<SAEforms.CommandHandler> logger, int partyId);
}
