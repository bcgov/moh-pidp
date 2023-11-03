namespace Pidp.Features.EndorsementRequests;

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

public class Approve
{
    public class Command : ICommand<IDomainResult>
    {
        public int PartyId { get; set; }
        public int EndorsementRequestId { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            this.RuleFor(x => x.PartyId).GreaterThan(0);
            this.RuleFor(x => x.EndorsementRequestId).GreaterThan(0);
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
            this.plrClient = plrClient;
            this.config = config;
            this.context = context;
        }

        public async Task<IDomainResult> HandleAsync(Command command)
        {
            var endorsementRequest = await this.context.EndorsementRequests
                .SingleOrDefaultAsync(request => request.Id == command.EndorsementRequestId);

            if (endorsementRequest == null)
            {
                return DomainResult.NotFound();
            }

            var result = endorsementRequest.Handle(command, this.clock);
            if (!result.IsSuccess)
            {
                return result;
            }

            if (endorsementRequest.Status == EndorsementRequestStatus.Completed)
            {
                // Both parties have approved, Request handshake is complete.
                await this.HandleMoaDesignation(endorsementRequest);
                this.context.Endorsements.Add(Endorsement.FromCompletedRequest(endorsementRequest));
            }

            await this.context.SaveChangesAsync();

            return DomainResult.Success();
        }

        private class PartyForEndorsement
        {
            public int Id { get; set; }
            public Guid UserId { get; set; }
            public string? Cpn { get; set; }
            public string? UserPrincipalName { get; set; }
        }

        private class PartyWithPlrStandingsDigest
        {
            public PartyWithPlrStandingsDigest(PartyForEndorsement partyForEndorsement, PlrStandingsDigest plrStandingsDigest)
            {
                this.PartyForEndorsement = partyForEndorsement;
                this.PlrStandingsDigest = plrStandingsDigest;
            }

            public PartyForEndorsement PartyForEndorsement { get; set; }
            public PlrStandingsDigest PlrStandingsDigest { get; set; }
        }

        // When a user with no College Licences is endorsed by a Licenced user in good standing, they recieve the "MOA" role.
        private async Task HandleMoaDesignation(EndorsementRequest request)
        {
            // get 2 parties: requesting and receiving
            var parties = await this.context.Parties
                .Where(party => party.Id == request.RequestingPartyId
                    || party.Id == request.ReceivingPartyId)
                .Select(party => new PartyForEndorsement()
                {
                    Id = party.Id,
                    UserId = party.PrimaryUserId,
                    Cpn = party.Cpn,
                    UserPrincipalName = party.Credentials
                        .Where(credential => credential.IdentityProvider == IdentityProviders.BCProvider)
                        .Select(credential => credential.IdpId)
                        .SingleOrDefault(),
                })
                .ToListAsync();

            if (!parties.Any(party => string.IsNullOrWhiteSpace(party.Cpn)))
            {
                // both Parties are unlicenced
                return;
            }

            // get all parties with a licence
            var licencedParties = parties.FindAll(party => !string.IsNullOrWhiteSpace(party.Cpn));
            var licencedPartiesInGoodStanding = new List<PartyWithPlrStandingsDigest>();
            var licencedPartiesInBadStanding = new List<PartyWithPlrStandingsDigest>();
            foreach (var licencedParty in licencedParties)
            {
                var licencedPartyStanding = await this.plrClient.GetStandingsDigestAsync(licencedParty.Cpn);
                if (licencedPartyStanding.HasGoodStanding)
                {
                    licencedPartiesInGoodStanding.Add(new PartyWithPlrStandingsDigest(licencedParty, licencedPartyStanding));
                }
                else
                {
                    licencedPartiesInBadStanding.Add(new PartyWithPlrStandingsDigest(licencedParty, licencedPartyStanding));
                }
            }
            if (licencedPartiesInGoodStanding.Count != 1)
            {
                // TODO: Log / other behaviour when either both Parties are in good standing or both Parties are in bad standing
                return;
            }

            var regulatedParty = licencedPartiesInGoodStanding.First().PartyForEndorsement;
            var regulatedPartyStanding = licencedPartiesInGoodStanding.First().PlrStandingsDigest;

            var unregulatedParty = parties.SingleOrDefault(party => string.IsNullOrWhiteSpace(party.Cpn))
                ?? licencedPartiesInBadStanding.SingleOrDefault()?.PartyForEndorsement;
            if (unregulatedParty == null)
            {
                return;
            }

            if (await this.keycloakClient.AssignAccessRoles(unregulatedParty.UserId, MohKeycloakEnrolment.MoaLicenceStatus))
            {
                this.context.BusinessEvents.Add(LicenceStatusRoleAssigned.Create(unregulatedParty.Id, MohKeycloakEnrolment.MoaLicenceStatus, this.clock.GetCurrentInstant()));
            }
            else
            {
                this.logger.LogMoaRoleAssignmentError(unregulatedParty.Id);
            }

            if (!string.IsNullOrWhiteSpace(unregulatedParty.UserPrincipalName)
                && regulatedPartyStanding
                    .With(IdentifierType.PhysiciansAndSurgeons, IdentifierType.Nurse, IdentifierType.Midwife)
                    .HasGoodStanding)
            {
                var endorsingCpns = await this.context.ActiveEndorsementRelationships(unregulatedParty.Id)
                    .Select(relationship => relationship.Party!.Cpn)
                    .ToListAsync();

                var endorsingPartiesStanding = await this.plrClient.GetAggregateStandingsDigestAsync(endorsingCpns);

                var unlicencedPartyBCProviderAttributes = new BCProviderAttributes(this.config.BCProviderClient.ClientId)
                    .SetIsMoa(true)
                    .SetEndorserData(endorsingPartiesStanding
                        .WithGoodStanding()
                        .With(IdentifierType.PhysiciansAndSurgeons, IdentifierType.Nurse, IdentifierType.Midwife)
                        .Cpns
                        .Append(regulatedParty.Cpn!));
                await this.bcProviderClient.UpdateAttributes(unregulatedParty.UserPrincipalName, unlicencedPartyBCProviderAttributes.AsAdditionalData());
            }

            if (!string.IsNullOrWhiteSpace(regulatedParty.UserPrincipalName))
            {
                var endorserCpns = await this.context.ActiveEndorsementRelationships(regulatedParty.Id)
                    .Select(relationship => relationship.Party!.Cpn)
                    .ToListAsync();

                var endorsingPartiesStanding = await this.plrClient.GetAggregateStandingsDigestAsync(endorserCpns);

                var licencedPartyBCProviderAttributes = new BCProviderAttributes(this.config.BCProviderClient.ClientId)
                    .SetIsMoa(true)
                    .SetEndorserData(endorsingPartiesStanding
                        .WithGoodStanding()
                        .With(IdentifierType.PhysiciansAndSurgeons, IdentifierType.Nurse, IdentifierType.Midwife)
                        .Cpns
                        .Append(regulatedParty.Cpn!));
                await this.bcProviderClient.UpdateAttributes(regulatedParty.UserPrincipalName, licencedPartyBCProviderAttributes.AsAdditionalData());
            }
        }
    }
}

public static partial class EndorsementRequestApprovalLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Error, "Error when assigning the MOA role to Party #{partyId}.")]
    public static partial void LogMoaRoleAssignmentError(this ILogger logger, int partyId);
}
