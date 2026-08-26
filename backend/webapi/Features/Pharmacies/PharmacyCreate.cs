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
        public string HealthAuthority { get; set; } = string.Empty;
        public string Address1 { get; set; } = string.Empty;
        public string Address2 { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Province { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string ManagerName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Fax { get; set; } = string.Empty;
        public string PharmaCareCode { get; set; } = string.Empty;
        public bool? IsCareConnectCompleted { get; set; }
        public DateTime? VerifiedCareConnectCompletedDate { get; set; }
        public string? VerifiedCareConnectCompleted { get; set; } = string.Empty;
        public int PartyId { get; set; }
        public required bool AckImmunizationScope { get; set; }
        public required bool AckAccessToVaccines { get; set; }
        public required bool AckPrivacy { get; set; }
        public required bool AckRemovalAccess { get; set; }
        public IFormFile? Evidence { get; set; }
    }

    public class CommandHandler(IClock clock, PidpDbContext context, IEmailService emailService, PidpConfiguration config) : IRequestHandler<Command, int>
    {
        private readonly IClock clock = clock;
        private readonly PidpDbContext context = context;
        private readonly IEmailService emailService = emailService;
        private readonly PidpConfiguration config = config;
        public async ValueTask<int> Handle(Command request, CancellationToken cancellationToken)
        {
            if (await this.context.Pharmacies.FirstOrDefaultAsync(p => p.Name == request.Name, cancellationToken) != null)
            {
                throw new InvalidOperationException("A pharmacy with this name already exists.");
            }

            if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.HealthAuthority) || string.IsNullOrWhiteSpace(request.Address1) || string.IsNullOrWhiteSpace(request.City) || string.IsNullOrWhiteSpace(request.Province) || string.IsNullOrWhiteSpace(request.PostalCode) || string.IsNullOrWhiteSpace(request.ManagerName) || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Phone))
            {
                throw new InvalidOperationException("All required fields must be filled.");
            }

            if (!request.AckImmunizationScope || !request.AckAccessToVaccines || !request.AckPrivacy || !request.AckRemovalAccess)
            {
                throw new InvalidOperationException("All acknowledgements must be accepted.");
            }

            if (request.Evidence != null)
            {
                if (request.Evidence.Length > 5 * 1024 * 1024)
                {
                    throw new InvalidOperationException("Evidence file size should be less than 5MB");
                }

                var allowedContentTypes = new[] { "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "application/pdf", "image/png", "image/jpeg" };
                if (!allowedContentTypes.Contains(request.Evidence.ContentType))
                {
                    throw new InvalidOperationException("Invalid file type for Evidence. Only Word docx, pdf, png, and jpg are allowed.");
                }
            }

            var pharmacy = new Pharmacy
            {
                Name = request.Name,
                HealthAuthority = request.HealthAuthority,
                Address1 = request.Address1,
                Address2 = request.Address2,
                City = request.City,
                Province = request.Province,
                PostalCode = request.PostalCode,
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
            this.context.BusinessEvents.Add(PharmacyAdded.Create(request.PartyId, request.Name, this.clock.GetCurrentInstant()));

            if (request.Evidence != null)
            {
                using var memoryStream = new MemoryStream();
                await request.Evidence.CopyToAsync(memoryStream, cancellationToken);
                var document = new Document
                {
                    Id = Guid.NewGuid(),
                    Data = memoryStream.ToArray(),
                    ContentType = request.Evidence.ContentType,
                    FileName = request.Evidence.FileName
                };
                this.context.Documents.Add(document);
                pharmacy.DocumentId = document.Id;

                var url = $"{this.config.ApplicationUrl}/api/documents/{document.Id}";
                var street = System.Text.RegularExpressions.Regex.Replace(request.Address1 ?? "", @"^(?i)(?:(?:(?:unit|suite)\b|[#\-,])\s*|[\w]+\s*[-,]\s*)*\d+[a-z]?\s+", "").Trim();
                var clinicName = $"{request.City} - {request.Name} - {street}";
                
                var email = new Email(
                    from: EmailService.PidpEmail,
                    to: this.config.Pharmacy.NotificationEmail,
                    subject: $"New Pharmacy Registration: {request.Name}",
                    body: $@"A new pharmacy has been registered.<br><br>
                    <b>Clinic Name:</b> {clinicName}<br>
                    <b>Name:</b> {request.Name}<br>
                    <b>Health Authority:</b> {request.HealthAuthority}<br>
                    <b>Address Line 1:</b> {request.Address1}<br>
                    <b>Address Line 2:</b> {request.Address2}<br>
                    <b>City:</b> {request.City}<br>
                    <b>Province:</b> {request.Province}<br>
                    <b>Postal Code:</b> {request.PostalCode}<br>
                    <b>Manager:</b> {request.ManagerName}<br>
                    <b>Email:</b> {request.Email}<br>
                    <b>Phone:</b> {request.Phone}<br>
                    <b>Fax:</b> {request.Fax}<br>
                    <b>PharmaCare Code:</b> {request.PharmaCareCode}<br><br>
                    <b>Evidence Document:</b> <a href=""{url}"">View Document</a>"
                );
                await this.emailService.SendAsync(email);
            }

            await this.context.SaveChangesAsync(cancellationToken);

            return pharmacy.Id;
        }
    }
}
