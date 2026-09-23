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

public class NpdpEforms
{
    public static IdentifierType[] AllowedIdentifierTypes => [IdentifierType.Pharmacist];

    public static bool IsEligible(PlrStandingsDigest partyPlrStanding)
    {
        return partyPlrStanding
            .With(AllowedIdentifierTypes)
            .HasGoodStanding;
    }

    public static async Task<bool> GrantAsync(int partyId, IEnumerable<Guid> userIds, IKeycloakAdministrationClient keycloakClient, PidpDbContext context, IClock clock)
    {
        foreach (var userId in userIds)
        {
            if (!await keycloakClient.AssignAccessRoles(userId, MohKeycloakEnrolment.NpdpEforms))
            {
                return false;
            }
        }

        context.AccessRequests.Add(new AccessRequest
        {
            PartyId = partyId,
            AccessTypeCode = AccessTypeCode.NpdpEforms,
            RequestedOn = clock.GetCurrentInstant()
        });

        context.BusinessEvents.Add(AccessRequestSubmitted.Create(partyId, AccessTypeCode.NpdpEforms.ToString(), clock.GetCurrentInstant()));

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
        IKeycloakAdministrationClient keycloakClient,
        ILogger<CommandHandler> logger,
        IPlrClient plrClient,
        PidpDbContext context) : ICommandHandler<Command, IDomainResult>
    {
        private readonly IClock clock = clock;
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
                    AlreadyEnroled = party.AccessRequests.Any(request => request.AccessTypeCode == AccessTypeCode.NpdpEforms),
                    AlreadyEnroledInSAEforms = party.AccessRequests.Any(request => request.AccessTypeCode == AccessTypeCode.SAEforms),
                    UserIds = party.Credentials
                        .Where(credential => credential.IdentityProvider == IdentityProviders.BCServicesCard)
                        .Select(credential => credential.UserId),
                    // SA eForms is granted to BC Provider credentials as well as BC Services Card ones
                    SAEformsUserIds = party.Credentials
                        .Where(credential => credential.IdentityProvider == IdentityProviders.BCServicesCard || credential.IdentityProvider == IdentityProviders.BCProvider)
                        .Select(credential => credential.UserId),
                    party.Email,
                    party.Cpn,
                })
                .SingleAsync();

            // UserIds holds only BC Services Card credentials, so an empty set means the
            // Party has none; the role has nowhere to land and the request must be denied.
            // A null CPN yields an empty digest, which is not good standing, so a Party without a
            // licence of their own is denied here too - there is no endorsement path onto this card.
            if (dto.AlreadyEnroled
                || dto.Email == null
                || !dto.UserIds.Any()
                || !IsEligible(await this.plrClient.GetStandingsDigestAsync(dto.Cpn)))
            {
                this.logger.LogAccessRequestDenied(command.PartyId);
                this.context.BusinessEvents.Add(AccessRequestFailed.Create(command.PartyId, AccessTypeCode.NpdpEforms.ToString(), this.clock.GetCurrentInstant()));
                await this.context.SaveChangesAsync();
                return DomainResult.Failed();
            }

            if (!await GrantAsync(command.PartyId, dto.UserIds, this.keycloakClient, this.context, this.clock))
            {
                return DomainResult.Failed();
            }

            // Pharmacists that apply for NPDP should get SA eForms access as well
            if (!dto.AlreadyEnroledInSAEforms
                && !await SAEforms.GrantAsync(command.PartyId, dto.SAEformsUserIds, this.keycloakClient, this.context, this.clock))
            {
                this.logger.LogPairedSAEformsGrantFailed(command.PartyId);
            }

            return DomainResult.Success();
        }
    }
}

public static partial class NpdpEformsLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Warning, "NPDP eForms Access Request for Party {partyId} denied; did not meet all prerequisites.")]
    public static partial void LogAccessRequestDenied(this ILogger<NpdpEforms.CommandHandler> logger, int partyId);

    [LoggerMessage(2, LogLevel.Warning, "SA eForms could not be granted to Party {partyId} alongside their NPDP eForms enrolment; the NPDP access itself was not affected.")]
    public static partial void LogPairedSAEformsGrantFailed(this ILogger<NpdpEforms.CommandHandler> logger, int partyId);
}
