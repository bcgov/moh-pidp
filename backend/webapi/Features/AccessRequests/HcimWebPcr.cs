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

public class HcimWebPcr
{

    private static readonly (IdentifierType IdentifierType, CollegeCode College)[] EligibleColleges =
    [
        (IdentifierType.PhysiciansAndSurgeons, CollegeCode.PhysiciansAndSurgeons),
        (IdentifierType.Nurse,                 CollegeCode.NursesAndMidwives),
        (IdentifierType.Midwife,               CollegeCode.NursesAndMidwives)
    ];

    public static IdentifierType[] AllowedIdentifierTypes => EligibleColleges.Select(entry => entry.IdentifierType).ToArray();

    public static bool IsEligible(PlrStandingsDigest partyPlrStanding) => partyPlrStanding.With(AllowedIdentifierTypes).HasGoodStanding;

    public static bool IsEligibleByEndorsement(PlrStandingsDigest endorsementPlrStanding) => endorsementPlrStanding.With(AllowedIdentifierTypes).HasGoodStanding;

    private static readonly Dictionary<CollegeCode, (string Id, string Name)> CollegeOrganizations = new()
    {
        [CollegeCode.PhysiciansAndSurgeons] = ("46013722", "College of Physicians and Surgeons of BC"),
        [CollegeCode.NursesAndMidwives] = ("12249113", "BC College of Nurses and Midwives")
    };

    public static CollegeCode? OrganizationCollegeFor(PlrStandingsDigest digest) => EligibleColleges
        .Where(entry => digest.With(entry.IdentifierType).HasGoodStanding)
        .Select(entry => (CollegeCode?)entry.College)
        .FirstOrDefault();

    public class Command : ICommand<IDomainResult>
    {
        public required int PartyId { get; set; }
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
                    AlreadyEnroled = party.AccessRequests.Any(request => request.AccessTypeCode == AccessTypeCode.HcimWebPcr),
                    HasBCServicesCardCredential = party.Credentials.Any(credential => credential.IdentityProvider == IdentityProviders.BCServicesCard),
                    UserId = party.Credentials
                        .Where(credential => credential.IdentityProvider == IdentityProviders.BCProvider)
                        .Select(credential => (Guid?)credential.UserId)
                        .FirstOrDefault(),
                    party.Cpn,
                })
                .SingleAsync();

            if (dto.AlreadyEnroled
                || !dto.HasBCServicesCardCredential
                || dto.UserId == null)
            {
                return await this.DenyAccess(command.PartyId);
            }

            // Held onto after the eligibility check because it also decides the organization below.
            PlrStandingsDigest plrStanding;

            if (dto.Cpn == null)
            {
                // Check status of Endorsements
                var endorsementCpns = await this.context.ActiveEndorsementRelationships(command.PartyId)
                    .Select(relationship => relationship.Party!.Cpn)
                    .ToListAsync();

                plrStanding = await this.plrClient.GetAggregateStandingsDigestAsync(endorsementCpns);

                if (!IsEligibleByEndorsement(plrStanding))
                {
                    return await this.DenyAccess(command.PartyId);
                }
            }
            else
            {
                plrStanding = await this.plrClient.GetStandingsDigestAsync(dto.Cpn);

                if (!IsEligible(plrStanding))
                {
                    return await this.DenyAccess(command.PartyId);
                }
            }

            if (!await this.keycloakClient.AssignAccessRoles(dto.UserId.Value, MohKeycloakEnrolment.HcimWebPcr))
            {
                this.logger.LogKeycloakRoleAssignmentFailed(command.PartyId);
                return DomainResult.Failed();
            }

            // Recorded before the Access Request is saved: a failure here leaves the Party un-enroled and
            // able to retry, rather than enroled with no organization for the Registry to read.
            if (!await this.AssignOrganizationAsync(command.PartyId, dto.UserId.Value, plrStanding))
            {
                return DomainResult.Failed();
            }

            this.context.AccessRequests.Add(new AccessRequest
            {
                PartyId = command.PartyId,
                AccessTypeCode = AccessTypeCode.HcimWebPcr,
                RequestedOn = this.clock.GetCurrentInstant()
            });

            this.context.BusinessEvents.Add(AccessRequestSubmitted.Create(command.PartyId, AccessTypeCode.HcimWebPcr.ToString(), this.clock.GetCurrentInstant()));

            await this.context.SaveChangesAsync();

            return DomainResult.Success();
        }


        private async Task<bool> AssignOrganizationAsync(int partyId, Guid userId, PlrStandingsDigest digest)
        {
            var collegeCode = OrganizationCollegeFor(digest);

            if (collegeCode == null
                || !CollegeOrganizations.TryGetValue(collegeCode.Value, out var organization))
            {
                // Unreachable while the eligibility check above passes: both read the same ordered table.
                this.logger.LogOrganizationNotResolved(partyId);
                return false;
            }

            if (!await this.keycloakClient.UpdateUser(userId, user => user.SetOrgDetails(organization.Id, organization.Name)))
            {
                this.logger.LogOrganizationAssignmentFailed(partyId, organization.Name);
                return false;
            }

            return true;
        }

        private async Task<IDomainResult> DenyAccess(int partyId)
        {
            this.logger.LogAccessRequestDenied();
            this.context.BusinessEvents.Add(AccessRequestFailed.Create(partyId, AccessTypeCode.HcimWebPcr.ToString(), this.clock.GetCurrentInstant()));
            await this.context.SaveChangesAsync();
            return DomainResult.Failed();
        }
    }
}

public static partial class HcimWebPcrLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Warning, "Provincial Client Registry Access Request denied due to the Party Record not meeting all prerequisites.")]
    public static partial void LogAccessRequestDenied(this ILogger<HcimWebPcr.CommandHandler> logger);

    [LoggerMessage(2, LogLevel.Error, "Provincial Client Registry Access Request failed; could not assign the Keycloak Access Roles for Party #{partyId}.")]
    public static partial void LogKeycloakRoleAssignmentFailed(this ILogger<HcimWebPcr.CommandHandler> logger, int partyId);

    [LoggerMessage(3, LogLevel.Error, "Provincial Client Registry Access Request failed; could not set the organization {organization} on the Keycloak User for Party #{partyId}.")]
    public static partial void LogOrganizationAssignmentFailed(this ILogger<HcimWebPcr.CommandHandler> logger, int partyId, string organization);

    [LoggerMessage(4, LogLevel.Error, "Provincial Client Registry Access Request failed; Party #{partyId} passed the eligibility check but no eligible college could be resolved for their organization.")]
    public static partial void LogOrganizationNotResolved(this ILogger<HcimWebPcr.CommandHandler> logger, int partyId);
}
