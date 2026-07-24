namespace Pidp.Features.Pharmacies;

using MediatR;
using Pidp.Data;
using Pidp.Models;
using Pidp.Models.Lookups;

public class Create
{
    public class Command : IRequest<int>
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string ManagerName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Fax { get; set; } = string.Empty;
        public string PharmaCareCode { get; set; } = string.Empty;
        public bool IsCareConnectCompleted { get; set; }
        public DateTime? VerifiedCareConnectCompletedDate { get; set; }
        public string? VerifiedCareConnectCompleted { get; set; } = string.Empty;
        public int PartyId { get; set; }
    }

    public class CommandHandler(PidpDbContext context) : IRequestHandler<Command, int>
    {
        private readonly PidpDbContext context = context;
        public async Task<int> Handle(Command request, CancellationToken cancellationToken)
        {
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
