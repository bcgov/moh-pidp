namespace Pidp.Features.Pharmacies;

using DomainResults.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using Mediator;

using Pidp.Data;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models;
using Pidp.Models.Lookups;

public class PharmacyClaim
{
    public class Command : IRequest<IDomainResult>
    {
        public int PharmacyId { get; set; }
        public int RequestingPartyId { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            this.RuleFor(x => x.PharmacyId).GreaterThan(0);
            this.RuleFor(x => x.RequestingPartyId).GreaterThan(0);
        }
    }

    public class CommandHandler(
        IClock clock,
        PidpDbContext context,
        IPlrClient plrClient) : IRequestHandler<Command, IDomainResult>
    {
        private readonly IClock clock = clock;
        private readonly PidpDbContext context = context;
        private readonly IPlrClient plrClient = plrClient;

        public async ValueTask<IDomainResult> Handle(Command command, CancellationToken cancellationToken)
        {
            var requestingParty = await this.context.Parties.SingleOrDefaultAsync(p => p.Id == command.RequestingPartyId, cancellationToken);

            if (requestingParty == null)
            {
                return DomainResult.Failed("Requesting party not found.");
            }

            var standings = await this.plrClient.GetStandingsDigestAsync(requestingParty.Cpn);
            if (!standings.With(IdentifierType.Pharmacist).HasGoodStanding)
            {
                return DomainResult.Failed("Only Pharmacists in good standing can claim a pharmacy.");
            }

            var pharmacy = await this.context.Pharmacies.Include(p => p.Staff).SingleOrDefaultAsync(p => p.Id == command.PharmacyId, cancellationToken);

            if (pharmacy == null)
            {
                return DomainResult.NotFound();
            }

            if (pharmacy.ManagerId != null)
            {
                return DomainResult.Failed("This pharmacy already has a manager and cannot be claimed.");
            }

            // Set the manager
            pharmacy.ManagerId = command.RequestingPartyId;

            // Add the lead role for the manager
            var leadRole = new PharmacyPartyRole
            {
                PartyId = command.RequestingPartyId,
                Pharmacy = pharmacy,
                Role = PharmacyRole.Lead,
                EffectiveStartDate = DateTime.UtcNow,
                EffectiveEndDate = DateTime.UtcNow.AddYears(10)
            };
            this.context.PharmacyPartyRoles.Add(leadRole);
            this.context.BusinessEvents.Add(PharmacyAdded.Create(command.RequestingPartyId, pharmacy.Name, this.clock.GetCurrentInstant()));

            await this.context.SaveChangesAsync(cancellationToken);

            return DomainResult.Success();
        }
    }
}
