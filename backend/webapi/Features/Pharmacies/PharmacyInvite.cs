namespace Pidp.Features.Pharmacies;

using DomainResults.Common;
using FluentValidation;
using Flurl;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Pidp.Data;
using Pidp.Infrastructure.HttpClients.Mail;
using Pidp.Infrastructure.Services;
using Pidp.Models.Lookups;

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
            this.RuleFor(x => x.RoleToAssign).IsInEnum();
            this.RuleFor(x => x.Emails).NotEmpty();
            this.RuleForEach(x => x.Emails).EmailAddress();
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

            var adminName = await context.Parties
                .Where(p => p.Id == request.RequestingPartyId)
                .Select(p => p.FirstName + " " + p.LastName)
                .SingleOrDefaultAsync(cancellationToken) ?? "an administrator";

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

                var baseUrl = config.ApplicationUrl;
                var baseLink = $"<a href=\"{baseUrl}\" target=\"_blank\" rel=\"noopener\">here</a>";

                var bcProviderUrl = baseUrl.AppendPathSegments("account", "bc-provider-application");
                var bcProviderLink = $"<a href=\"{bcProviderUrl}\" target=\"_blank\" rel=\"noopener\">Link Account</a>";

                var enrolUrl = baseUrl.AppendPathSegments("access", "immsbc", "pharmacy-enrol", token);
                var enrolLink = $"<a href=\"{enrolUrl}\" target=\"_blank\" rel=\"noopener\">this link</a>";

                var pidpSupportEmail = $"<a href=\"mailto:{EmailService.PidpEmail}\">{EmailService.PidpEmail}</a>";

                var email = new Email(
                    from: EmailService.PidpEmail,
                    to: emailAddress,
                    subject: $"Invitation to join {pharmacyName} on ImmsBC",
                    body: $@"Hello,
<br>You are receiving this email because {adminName} has added you as a staff member at {pharmacyName}.<br>
<br>To accept this invitation and registered at {pharmacyName} please complete the following steps:
<br>
<br>For users without a BC Provider Account
<br>1. Create a BC Provider Account {baseLink}
<br>2. Select login with BC Services Card app and follow the prompts to create your account.<br>3. At the top right of the page select Account and Account Linking.<br>4. Select the BCProvider time, {bcProviderLink}
<br>5. Select First Time setup towards the bottom of the page.<br>6. Enter a password for your BC Provider account.<br>7. One logged in select {enrolLink}.<br>8. Confirm you've completed the privacy and security training from CareConnect and Enrol.<br>9. You will see the success message.<br>10. Select OK to confirm you're successfully been added to the pharmacy.<br>
<br>For users with an existing BC Provider Account
<br>1. Login to your OneHealthID with your BC Services Card app {baseLink}.<br>
<br>2. If it is your first time logging in, complete the contact information, and license information (for pharmacists).<br>
<br>3. Complete the BC Provider {bcProviderLink}. If you're prompted to change your password, your BC Provider account is already linked.<br>
<br>4. Confirm you’ve completed privacy and security training from CareConnect and Enrol by clicking on {enrolLink}.<br>
<br>5. You will see the success message.<br>6. Select OK to confirm you're successfully been added to the pharmacy.<br>
<br>For additional support with onboarding contact the OneHealthID Service desk by email at {pidpSupportEmail}.<br>For all ImmsBC related questions please contact the VaxBC.<br>Thank you.");

                await emailService.SendAsync(email);
            }

            return DomainResult.Success();
        }
    }
}
