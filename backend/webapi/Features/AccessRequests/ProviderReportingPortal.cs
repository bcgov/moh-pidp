namespace Pidp.Features.AccessRequests;

using DomainResults.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NodaTime;

using Pidp.Data;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models;
using Pidp.Models.Lookups;

public class ProviderReportingPortal
{
    public static IdentifierType[] AllowedIdentifierTypes => new[] { IdentifierType.PhysiciansAndSurgeons };

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
        private readonly IKeycloakAdministrationClient keycloakClient;
        private readonly ILogger logger;
        private readonly IPlrClient plrClient;
        private readonly PidpDbContext context;

        public CommandHandler(
            IClock clock,
            IKeycloakAdministrationClient keycloakClient,
            ILogger<CommandHandler> logger,
            IPlrClient plrClient,
            PidpDbContext context)
        {
            this.clock = clock;
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
                    AlreadyEnroled = party.AccessRequests.Any(request => request.AccessTypeCode == AccessTypeCode.ProviderReportingPortal),
                    party.Credentials.Single(credential => credential.IdentityProvider == IdentityProviders.BCProvider).UserId,
                    party.Cpn,
                })
                .SingleAsync();

            var filteredPlrDigest = (await this.plrClient.GetStandingsDigestAsync(dto.Cpn))
                .With(AllowedIdentifierTypes);

            if (dto.AlreadyEnroled
                || !filteredPlrDigest
                    .HasGoodStanding)
            {
                this.logger.LogProviderReportingPortalAccessRequestDenied();
                return DomainResult.Failed();
            }

            var prpAuthorization = await this.context.PrpAuthorizedLicences
                .SingleOrDefaultAsync(authorizedLicence => filteredPlrDigest.LicenceNumbers.Contains(authorizedLicence.LicenceNumber));

            if (prpAuthorization == null)
            {
                this.logger.LogProviderReportingPortalUnauthorizedLicence(command.PartyId, filteredPlrDigest.LicenceNumbers);
                return DomainResult.Failed();
            }

            if (!await this.keycloakClient.AssignAccessRoles(dto.UserId, MohKeycloakEnrolment.ProviderReportingPortal))
            {
                return DomainResult.Failed();
            }

            this.context.AccessRequests.Add(new AccessRequest
            {
                PartyId = command.PartyId,
                AccessTypeCode = AccessTypeCode.ProviderReportingPortal,
                RequestedOn = this.clock.GetCurrentInstant()
            });

            prpAuthorization.Claimed = true;

            await this.context.SaveChangesAsync();

            return DomainResult.Success();
        }
    }
}

public static partial class ProviderReportingPortalLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Warning, "Provider Reporting Portal enrolment denied due to the Party Record not meeting all prerequisites.")]
    public static partial void LogProviderReportingPortalAccessRequestDenied(this ILogger logger);

    [LoggerMessage(2, LogLevel.Warning, "Provider Reporting Portal enrolment denied; Party #{partyId} with Licence Number(s) {licenceNumbers} are not pre-authorized.")]
    public static partial void LogProviderReportingPortalUnauthorizedLicence(this ILogger logger, int partyId, IEnumerable<string> licenceNumbers);
}
