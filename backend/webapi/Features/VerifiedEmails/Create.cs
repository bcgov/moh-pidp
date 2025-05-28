namespace Pidp.Features.VerifiedEmail;

using System.Text.Json.Serialization;
using FluentValidation;
using Flurl;
using HybridModelBinding;
using MassTransit;
using MediatR;
using NodaTime;

using Pidp.Data;
using Pidp.Extensions;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.Mail;
using Pidp.Infrastructure.Services;
using Pidp.Models;
using Pidp.Models.DomainEvents;

public class Create
{
    public class Command : ICommand
    {
        [JsonIgnore]
        [HybridBindProperty(Source.Route)]
        public int PartyId { get; set; }
        public string Email { get; set; } = string.Empty;
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
        IClock clock,
        IEmailService emailService,
        PidpConfiguration config,
        PidpDbContext context) : ICommandHandler<Command>
    {
        private readonly string applicationUrl = config.ApplicationUrl;
        private readonly IClock clock = clock;
        private readonly IEmailService emailService = emailService;
        private readonly PidpDbContext context = context;

        public async Task HandleAsync(Command command)
        {
            await this.SendVerificationEmailAsync(command.Email, Guid.NewGuid());

            return;
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
