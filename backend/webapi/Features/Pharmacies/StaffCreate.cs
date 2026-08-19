namespace Pidp.Features.Pharmacies;

using Mediator;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using Pidp.Data;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.Services;
using Pidp.Models;
using Pidp.Models.Lookups;

public class StaffCreate
{
    public class Command : IRequest
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public Guid Token { get; set; }
        [System.Text.Json.Serialization.JsonIgnore]
        public int PartyId { get; set; }
        public bool PrivacyTrainingAcknowledged { get; set; }
    }

    public class CommandHandler : IRequestHandler<Command>
    {
        private readonly PidpDbContext context;
        private readonly IClock clock;
        private readonly IBCProviderService bcProviderService;

        public CommandHandler(PidpDbContext context, IClock clock, IBCProviderService bcProviderService)
        {
            this.context = context;
            this.clock = clock;
            this.bcProviderService = bcProviderService;
        }

        public async ValueTask<Unit> Handle(Command request, CancellationToken cancellationToken)
        {
            var enrolment = await this.context.PharmacyEnrolments
                .SingleOrDefaultAsync(enrolment => enrolment.Token == request.Token, cancellationToken);

            if (enrolment == null)
            {
                throw new KeyNotFoundException("Enrolment token not found or has already been used.");
            }

            if (!request.PrivacyTrainingAcknowledged)
            {
                throw new InvalidOperationException("Privacy and security training must be acknowledged.");
            }

            var now = this.clock.GetCurrentInstant().ToDateTimeUtc();
            if (enrolment.EffectiveEndDate < now)
            {
                throw new InvalidOperationException("Enrolment token has expired.");
            }

            var existingRole = await this.context.PharmacyPartyRoles
                .AnyAsync(role => role.PartyId == request.PartyId && role.PharmacyId == enrolment.PharmacyId, cancellationToken);

            if (existingRole)
            {
                throw new InvalidOperationException("User already associated with this pharmacy.");
            }

            var newRole = new PharmacyPartyRole
            {
                PartyId = request.PartyId,
                PharmacyId = enrolment.PharmacyId,
                Role = enrolment.Role,
                EffectiveStartDate = now,
                PrivacyTrainingAckDate = now
            };

            this.context.PharmacyPartyRoles.Add(newRole);

            var pharmacyName = await this.context.Pharmacies
                .Where(p => p.Id == enrolment.PharmacyId)
                .Select(p => p.Name)
                .SingleOrDefaultAsync(cancellationToken) ?? string.Empty;

            this.context.BusinessEvents.Add(PharmacyStaffChanged.Create(request.PartyId, pharmacyName, this.clock.GetCurrentInstant()));

            await this.context.SaveChangesAsync(cancellationToken);

            var hasBcProvider = await this.context.Credentials
                .AnyAsync(c => c.PartyId == request.PartyId && c.IdentityProvider == IdentityProviders.BCProvider, cancellationToken);

            if (!hasBcProvider)
            {
                throw new InvalidOperationException("Please link a BC Provider credential via /account/bc-provider-application.");
            }

            await this.bcProviderService.UpdatePharmStaffAttributes(request.PartyId, cancellationToken);
            return Unit.Value;
        }
    }
}
