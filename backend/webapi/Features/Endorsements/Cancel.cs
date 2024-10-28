namespace Pidp.Features.Endorsements;

using DomainResults.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NodaTime;

using Pidp.Data;
using Pidp.Extensions;
using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models;
using Pidp.Models.DomainEvents;


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
            await this.context.SaveChangesAsync(); // This double Save is deliberate; we need to persist changes to the Endorsement Relationships in the database before we can calculate the EndorserData in the Domain Events.

            // TODO: We don't actually need the whole Party, just the Id, PrimaryUserId and Cpn.
            // Consider a generic way of attaching domain events so we don't have to fetch the entire model.
            var parties = await this.context.EndorsementRelationships
                .Include(relationship => relationship.Party!.Credentials)
                .Where(relationship => relationship.EndorsementId == command.EndorsementId)
                .Select(relationship => relationship.Party!)
                .ToListAsync();

            var party1 = parties[0];
            var party2 = parties[1];

            if (await this.plrClient.GetStandingAsync(party1.Cpn))
            {
                party2.DomainEvents.Add(new EndorsementStandingUpdated(party2.Id));

                await this.RecalculateMoaRole(party2);
            }

            if (await this.plrClient.GetStandingAsync(party2.Cpn))
            {
                party1.DomainEvents.Add(new EndorsementStandingUpdated(party1.Id));

                await this.RecalculateMoaRole(party1);
            }

            await this.context.SaveChangesAsync();

            return DomainResult.Success();
        }

        private async Task RecalculateMoaRole(Party party)
        {
            var endorsingCpns = await this.context.ActiveEndorsingParties(party.Id)
                .Select(party => party.Cpn)
                .ToListAsync();
            if ((await this.plrClient.GetAggregateStandingsDigestAsync(endorsingCpns)).HasGoodStanding)
            {
                // User is still endorsed by a Licenced Party in good standing.
                return;
            }

            var role = await this.keycloakClient.GetClientRole(MohKeycloakEnrolment.MoaLicenceStatus.ClientId, MohKeycloakEnrolment.MoaLicenceStatus.AccessRoles.First());
            if (await this.keycloakClient.RemoveClientRole(party.PrimaryUserId, role!))
            {
                this.context.BusinessEvents.Add(LicenceStatusRoleUnassigned.Create(party.Id, MohKeycloakEnrolment.MoaLicenceStatus, this.clock.GetCurrentInstant()));
            }
            else
            {
                this.logger.LogMoaRoleAssignmentError(party.Id);
            }
        }
    }
}

public static partial class EndorsementCancelLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Error, "Error when unassigning the MOA role to Party #{partyId}.")]
    public static partial void LogMoaRoleAssignmentError(this ILogger<Cancel.CommandHandler> logger, int partyId);
}
