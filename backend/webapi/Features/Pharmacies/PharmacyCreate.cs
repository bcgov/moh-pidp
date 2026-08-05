namespace Pidp.Features.Pharmacies;

using MediatR;
using Pidp.Data;
using Pidp.Models;
using Pidp.Models.Lookups;
using Microsoft.EntityFrameworkCore;

public class PharmacyCreate
{
    public class Command : IRequest<int>
    {
        public required int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string ManagerName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Fax { get; set; } = string.Empty;
        public string PharmaCareCode { get; set; } = string.Empty;
        public required bool IsCareConnectCompleted { get; set; }
        public DateTime? VerifiedCareConnectCompletedDate { get; set; }
        public string? VerifiedCareConnectCompleted { get; set; } = string.Empty;
        public int PartyId { get; set; }
        public required bool AckImmunizationScope { get; set; }
        public required bool AckAccessToVaccines { get; set; }
        public required bool AckPrivacy { get; set; }
        public required bool AckRemovalAccess { get; set; }
    }

    public class CommandHandler(PidpDbContext context) : IRequestHandler<Command, int>
    {
        private readonly PidpDbContext context = context;
        public async Task<int> Handle(Command request, CancellationToken cancellationToken)
        {
            if (await this.context.Pharmacies.FirstOrDefaultAsync(p => p.Name == request.Name, cancellationToken) != null)
            {
                throw new InvalidOperationException("A pharmacy with this name already exists.");
            }

            if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Address) || string.IsNullOrWhiteSpace(request.ManagerName) || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Phone))
            {
                throw new InvalidOperationException("All required fields must be filled.");
            }

            if (!request.AckImmunizationScope || !request.AckAccessToVaccines || !request.AckPrivacy || !request.AckRemovalAccess)
            {
                throw new InvalidOperationException("All acknowledgements must be accepted.");
            }

            var pharmacy = new Pharmacy
            {
                Name = request.Name,
                Address = request.Address,
                ManagerName = request.ManagerName,
                Email = request.Email,
                Phone = request.Phone,
                Fax = request.Fax,
                PharmaCareCode = request.PharmaCareCode,
                IsCareConnectCompleted = false
            };

            this.context.Pharmacies.Add(pharmacy);

            var partyPharmacyRole = new PharmacyPartyRole
            {
                PartyId = request.PartyId,
                Pharmacy = pharmacy,
                Role = PharmacyRole.Admin
            };
            this.context.PharmacyPartyRoles.Add(partyPharmacyRole);

            await this.context.SaveChangesAsync(cancellationToken);

            return pharmacy.Id;
        }
    }
}
