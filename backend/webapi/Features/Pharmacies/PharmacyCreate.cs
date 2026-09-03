namespace Pidp.Features.Pharmacies;

using Mediator;
using Pidp.Data;
using Pidp.Models;
using Pidp.Models.Lookups;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using Pidp.Infrastructure.HttpClients.Mail;
using Pidp.Infrastructure.Services;

public class PharmacyCreate
{
    public class Command : IRequest<int>
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Fax { get; set; } = string.Empty;
        public string PharmaCareCode { get; set; } = string.Empty;
        public int ManagerId { get; set; }

    }

    public class CommandHandler(IClock clock, PidpDbContext context) : IRequestHandler<Command, int>
    {
        private readonly IClock clock = clock;
        private readonly PidpDbContext context = context;
        public async ValueTask<int> Handle(Command request, CancellationToken cancellationToken)
        {
            if (await this.context.Pharmacies.FirstOrDefaultAsync(p => p.Name == request.Name, cancellationToken) != null)
            {
                throw new InvalidOperationException("A pharmacy with this name already exists.");
            }

            if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Address) || string.IsNullOrWhiteSpace(request.Email))
            {
                throw new InvalidOperationException("Name, Address, and Email must be filled.");
            }

            if (string.IsNullOrWhiteSpace(request.PharmaCareCode) || request.PharmaCareCode.Length != 10 || !request.PharmaCareCode.StartsWith("BC"))
            {
                throw new InvalidOperationException("PharmaCare Code must be 10 characters and start with 'BC'.");
            }




            var pharmacy = new Pharmacy
            {
                Name = request.Name,
                Address = request.Address,
                Email = request.Email,
                Phone = request.Phone,
                Fax = request.Fax,
                PharmaCareCode = request.PharmaCareCode,
                ManagerId = request.ManagerId
            };

            this.context.Pharmacies.Add(pharmacy);

            var partyPharmacyRole = new PharmacyPartyRole
            {
                PartyId = request.ManagerId,
                Pharmacy = pharmacy,
                Role = PharmacyRole.Lead,
                EffectiveStartDate = DateTime.UtcNow,
                EffectiveEndDate = DateTime.UtcNow.AddYears(10)
            };
            this.context.PharmacyPartyRoles.Add(partyPharmacyRole);
            this.context.BusinessEvents.Add(PharmacyAdded.Create(request.ManagerId, request.Name, this.clock.GetCurrentInstant()));


            await this.context.SaveChangesAsync(cancellationToken);

            return pharmacy.Id;
        }
    }
}
