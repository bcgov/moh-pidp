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

                var templatePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pharmacy-invite-email.template");
                if (!System.IO.File.Exists(templatePath))
                {
                    throw new System.IO.FileNotFoundException($"Email template not found at {templatePath}");
                }
                var templateBody = await System.IO.File.ReadAllTextAsync(templatePath, cancellationToken);

                var body = templateBody
                    .Replace("{adminName}", adminName)
                    .Replace("{pharmacyName}", pharmacyName)
                    .Replace("{baseUrl}", baseUrl)
                    .Replace("{baseLink}", baseLink)
                    .Replace("{bcProviderUrl}", bcProviderUrl)
                    .Replace("{bcProviderLink}", bcProviderLink)
                    .Replace("{enrolUrl}", enrolUrl)
                    .Replace("{enrolLink}", enrolLink)
                    .Replace("{pidpSupportEmail}", pidpSupportEmail);

                var email = new Email(
                    from: EmailService.PidpEmail,
                    to: emailAddress,
                    subject: $"Invitation to join {pharmacyName} on ImmsBC",
                    body: body);

                await emailService.SendAsync(email);
            }

            return DomainResult.Success();
        }
    }
}
