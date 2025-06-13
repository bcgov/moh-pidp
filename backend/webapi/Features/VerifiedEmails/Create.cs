namespace Pidp.Features.VerifiedEmails;

using FluentValidation;
using Flurl;
using HybridModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

using Pidp.Data;
using Pidp.Infrastructure.HttpClients.Mail;
using Pidp.Infrastructure.Services;
using Pidp.Models;

public class Create
{
    public class Command : ICommand<Model>
    {
        [JsonIgnore]
        [HybridBindProperty(Source.Route)]
        public int PartyId { get; set; }
        public string Email { get; set; } = string.Empty;
    }

    public class Model
    {
        public bool IsVerified { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            this.RuleFor(x => x.PartyId).GreaterThan(0);
            this.RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }

    public class CommandHandler(
        IEmailService emailService,
        PidpConfiguration config,
        PidpDbContext context) : ICommandHandler<Command, Model>
    {
        private readonly string applicationUrl = config.ApplicationUrl;
        private readonly IEmailService emailService = emailService;
        private readonly PidpDbContext context = context;

        public async Task<Model> HandleAsync(Command command)
        {
            var verifiedEmail = await this.context.VerifiedEmails
                .Where(verifiedEmail => verifiedEmail.PartyId == command.PartyId
                    && verifiedEmail.Email.ToLower() == command.Email.ToLower())
                .SingleOrDefaultAsync();

            if (verifiedEmail == null)
            {
                verifiedEmail = new VerifiedEmail
                {
                    PartyId = command.PartyId,
                    Token = Guid.NewGuid(),
                    Email = command.Email
                };

                this.context.VerifiedEmails.Add(verifiedEmail);
                await this.context.SaveChangesAsync();
            }

            if (!verifiedEmail.IsVerified)
            {
                await this.SendVerificationEmailAsync(verifiedEmail.Email, verifiedEmail.Token);
            }

            return new Model
            {
                IsVerified = verifiedEmail.IsVerified
            };
        }

        private async Task SendVerificationEmailAsync(string recipientEmail, Guid token)
        {
            var url = $"{this.applicationUrl}/access/external-accounts".SetQueryParam("email-verification-token", token);
            var link = $"<a href=\"{url}\" target=\"_blank\" rel=\"noopener noreferrer\">this link</a>";

            var email = new Email(
                from: EmailService.PidpEmail,
                to: recipientEmail,
                subject: $"OneHealthID Email Verification Request",
                body: $@"Hello,
<br>
<br>To complete your email verification, use {link} to log into the OneHealthID Service.");

            await this.emailService.SendAsync(email);
        }
    }
}
