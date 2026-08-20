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

            if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Address) || string.IsNullOrWhiteSpace(request.ManagerName) || string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Phone))
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
                var email = new Email(
                    from: EmailService.PidpEmail,
                    to: this.config.Pharmacy.NotificationEmail,
                    subject: $"New Pharmacy Registration: {request.Name}",
                    body: $@"A new pharmacy has been registered.<br><br>
                    <b>Name:</b> {request.Name}<br>
                    <b>Address:</b> {request.Address}<br>
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
