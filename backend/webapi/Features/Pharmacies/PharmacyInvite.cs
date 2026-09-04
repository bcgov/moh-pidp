namespace Pidp.Features.Pharmacies;

using FluentValidation;
using Flurl;
using DomainResults.Common;
using Microsoft.EntityFrameworkCore;

using Pidp.Data;
using Pidp.Models.Lookups;
using Pidp.Infrastructure.Services;
using Pidp.Infrastructure.HttpClients.Mail;
using Mediator;

public class PharmacyInvite
{
    public class Command : ICommand<IDomainResult>
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public int PharmacyId { get; set; }
        
        [System.Text.Json.Serialization.JsonRequired]
        public PharmacyRole RoleToAssign { get; set; }
        
        public List<string> Emails { get; set; } = new();
        
        [System.Text.Json.Serialization.JsonIgnore]
        public int RequestingPartyId { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            this.RuleFor(x => x.PharmacyId).GreaterThan(0);
            this.RuleFor(x => x.RoleToAssign).IsInEnum();
            this.RuleFor(x => x.Emails).NotEmpty();
            this.RuleForEach(x => x.Emails).EmailAddress();
            this.RuleFor(x => x.RequestingPartyId).GreaterThan(0);
        }
    }

    public class CommandHandler(
        PidpDbContext context,
        IEmailService emailService,
        PidpConfiguration config,
        IMediator mediator) : ICommandHandler<Command, IDomainResult>
    {
        public async ValueTask<IDomainResult> Handle(Command request, CancellationToken cancellationToken)
        {
            var pharmacy = await context.Pharmacies
                .Where(p => p.Id == request.PharmacyId)
                .Select(p => p.Name)
                .SingleOrDefaultAsync(cancellationToken);

            if (pharmacy == null)
            {
                return DomainResult.NotFound();
            }

            var partyIsAdmin = await context.PharmacyPartyRoles
                .AnyAsync(role => role.PartyId == request.RequestingPartyId
                               && role.PharmacyId == request.PharmacyId
                               && (role.Role == PharmacyRole.Admin || role.Role == PharmacyRole.Lead),
                          cancellationToken);

            if (!partyIsAdmin)
            {
                return DomainResult.Unauthorized();
            }

            var pharmacyParts = pharmacy.Split('-');
            var pharmacyName = pharmacyParts.Length >= 2 ? pharmacyParts[1].Trim() : pharmacy;

            foreach (var emailAddress in request.Emails)
            {
                var tokenCmd = new GenerateEnrolmentToken.Command
                {
                    PharmacyId = request.PharmacyId,
                    RoleToAssign = request.RoleToAssign,
                    RequestingPartyId = request.RequestingPartyId
                };

                var token = await mediator.Send(tokenCmd, cancellationToken);
                
                string url = config.ApplicationUrl.AppendPathSegments("access", "immsbc", "pharmacy-enrol", token);
                var link = $"<a href=\"{url}\" target=\"_blank\" rel=\"noopener noreferrer\">this link</a>";
                var pidpSupportEmail = $"<a href=\"mailto:{EmailService.PidpEmail}\">{EmailService.PidpEmail}</a>";

                var email = new Email(
                    from: EmailService.PidpEmail,
                    to: emailAddress,
                    subject: $"Invitation to join {pharmacyName} on ImmsBC",
                    body: $@"Hello,
<br>You are receiving this email because an administrator invited you to join {pharmacyName} on ImmsBC.
<br>
<br>To accept this invitation and register with ImmsBC, please use {link} to log into the OneHealthID Service with your BC Services Card.
<br>
<br>For additional support, contact the OneHealthID Service desk:
<br>
<br>&emsp; By email at {pidpSupportEmail}
<br>
<br>Thank you.");

                await emailService.SendAsync(email);
            }

            return DomainResult.Success();
        }
    }
}
