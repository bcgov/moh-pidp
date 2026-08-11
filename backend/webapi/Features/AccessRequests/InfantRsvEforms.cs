namespace Pidp.Features.AccessRequests;

using DomainResults.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NodaTime;

using Pidp.Data;
using Pidp.Extensions;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models;
using Pidp.Models.Lookups;

public class InfantRsvEforms
{
    public static IdentifierType[] AllowedIdentifierTypes => [IdentifierType.PhysiciansAndSurgeons, IdentifierType.Nurse];

    public static bool IsEligible(PlrStandingsDigest partyPlrStanding)
    {
        return partyPlrStanding
            .With(AllowedIdentifierTypes)
            .HasGoodStanding || partyPlrStanding.IsCpsPostgrad;
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
                    AlreadyEnroled = party.AccessRequests.Any(request => request.AccessTypeCode == AccessTypeCode.InfantRsvEforms),
                    UserIds = party.Credentials
                        .Where(credential => credential.IdentityProvider == IdentityProviders.BCServicesCard || credential.IdentityProvider == IdentityProviders.BCProvider)
                        .Select(credential => credential.UserId),
                    party.Email,
                    party.Cpn,
                })
                .SingleAsync();

            if (dto.AlreadyEnroled
                || dto.Email == null)
            {
                this.logger.LogAccessRequestDenied(command.PartyId);
                return DomainResult.Failed();
            }

            if (dto.Cpn == null)
            {
                // Check status of Endorsements
                var endorsementCpns = await this.context.ActiveEndorsementRelationships(command.PartyId)
                    .Select(relationship => relationship.Party!.Cpn)
                    .ToListAsync();

                var endorsementPlrStanding = await this.plrClient.GetAggregateStandingsDigestAsync(endorsementCpns);

                if (!endorsementPlrStanding.With(ProviderRoleType.MedicalDoctor).HasGoodStanding &&
                    !endorsementPlrStanding.With(IdentifierType.Nurse).HasGoodStanding)
                {
                    this.logger.LogAccessRequestDenied(command.PartyId);
                    return DomainResult.Failed();
                }
            }
            else
            {
                if (!IsEligible(await this.plrClient.GetStandingsDigestAsync(dto.Cpn)))
                {
                    this.logger.LogAccessRequestDenied(command.PartyId);
                    return DomainResult.Failed();
                }
            }

            foreach (var userId in dto.UserIds)
            {
                if (!await this.keycloakClient.AssignAccessRoles(userId, MohKeycloakEnrolment.InfantRsvEforms))
                {
                    return DomainResult.Failed();
                }
            }

            this.context.AccessRequests.Add(new AccessRequest
            {
                PartyId = command.PartyId,
                AccessTypeCode = AccessTypeCode.InfantRsvEforms,
                RequestedOn = this.clock.GetCurrentInstant()
            });

            await this.context.SaveChangesAsync();

            return DomainResult.Success();
        }
    }
}

public static partial class InfantRsvEformsLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Warning, "Infant RSV eForms Access Request for Party {partyId} denied; did not meet all prerequisites.")]
    public static partial void LogAccessRequestDenied(this ILogger<InfantRsvEforms.CommandHandler> logger, int partyId);
}
