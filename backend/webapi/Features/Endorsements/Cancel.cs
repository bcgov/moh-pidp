namespace Pidp.Features.Endorsements;

using DomainResults.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NodaTime;

using Pidp.Data;
using Pidp.Extensions;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.BCProvider;
using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models;

public class Cancel
{
    public class Command : ICommand<IDomainResult>
    {
        public int EndorsementId { get; set; }
        public int PartyId { get; set; }
    }

    public class ComandValidator : AbstractValidator<Command>
    {
        public ComandValidator()
        {
            this.RuleFor(x => x.EndorsementId).GreaterThan(0);
            this.RuleFor(x => x.PartyId).GreaterThan(0);
        }
    }

    public class CommandHandler : ICommandHandler<Command, IDomainResult>
    {
        private readonly IBCProviderClient bcProviderClient;
        private readonly IClock clock;
        private readonly IKeycloakAdministrationClient keycloakClient;
        private readonly ILogger logger;
        private readonly IPlrClient plrClient;
        private readonly PidpConfiguration config;
        private readonly PidpDbContext context;

        public CommandHandler(
            IBCProviderClient bcProviderClient,
            IClock clock,
            IKeycloakAdministrationClient keycloakClient,
            ILogger<CommandHandler> logger,
            IPlrClient plrClient,
            PidpConfiguration config,
            PidpDbContext context)
        {
            this.bcProviderClient = bcProviderClient;
            this.clock = clock;
            this.keycloakClient = keycloakClient;
            this.logger = logger;
            this.config = config;
            this.context = context;
            this.plrClient = plrClient;
        }

        public async Task<IDomainResult> HandleAsync(Command command)
        {
            var endorsement = await this.context.EndorsementRelationships
                .Where(relationship => relationship.EndorsementId == command.EndorsementId
                    && relationship.PartyId == command.PartyId)
                .Select(relationship => relationship.Endorsement)
                .SingleOrDefaultAsync();

            if (endorsement == null)
            {
                return DomainResult.NotFound();
            }

            endorsement.Active = false;
            await this.context.SaveChangesAsync();

            var parties = await this.context.EndorsementRelationships
                .Where(relationship => relationship.EndorsementId == command.EndorsementId)
                .Select(relationship => new
                {
                    relationship.PartyId,
                    relationship.Party!.Cpn,
                    UserId = relationship.Party.PrimaryUserId,
                    UserPrincipalName = relationship.Party!.Credentials
                        .Where(credential => credential.IdentityProvider == IdentityProviders.BCProvider)
                        .Select(credential => credential.IdpId)
                        .SingleOrDefault(),
                })
                .ToListAsync();

            if (parties.Count(party => party.Cpn == null) != 1)
            {
                // Either both parties have licences or both don't.
                return DomainResult.Success();
            }

            var unlicencedParty = parties.Single(party => party.Cpn == null);

            var endorsingCpns = await this.context.ActiveEndorsementRelationships(unlicencedParty.PartyId)
                .Select(relationship => relationship.Party!.Cpn)
                .ToListAsync();

            var endorsingPlrDigest = await this.plrClient.GetAggregateStandingsDigestAsync(endorsingCpns);

            if (!endorsingPlrDigest.HasGoodStanding)
            {
                var role = await this.keycloakClient.GetClientRole(MohKeycloakEnrolment.MoaLicenceStatus.ClientId, MohKeycloakEnrolment.MoaLicenceStatus.AccessRoles.First());
                if (await this.keycloakClient.RemoveClientRole(unlicencedParty.UserId, role!))
                {
                    this.context.BusinessEvents.Add(LicenceStatusRoleUnassigned.Create(unlicencedParty.PartyId, MohKeycloakEnrolment.MoaLicenceStatus, this.clock.GetCurrentInstant()));
                    await this.context.SaveChangesAsync();
                }
                else
                {
                    this.logger.LogMoaRoleAssignmentError(unlicencedParty.PartyId);
                }

                if (!string.IsNullOrWhiteSpace(unlicencedParty.UserPrincipalName))
                {
                    var endorseeBcProviderAttributes = new BCProviderAttributes(this.config.BCProviderClient.ClientId).SetIsMoa(false);
                    await this.bcProviderClient.UpdateAttributes(unlicencedParty.UserPrincipalName, endorseeBcProviderAttributes.AsAdditionalData());
                }
            }

            return DomainResult.Success();
        }
    }
}

public static partial class EndorsementCancelLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Error, "Error when unassigning the MOA role to Party #{partyId}.")]
    public static partial void LogMoaRoleAssignmentError(this ILogger logger, int partyId);
}
