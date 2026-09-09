#pragma warning disable CA1862, CA1304

namespace Pidp.Features.VerifiedEmails;

using FluentValidation;
using Flurl;
using HybridModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        ILogger<CommandHandler> logger,
        PidpDbContext context) : ICommandHandler<Command, Model>
    {
        private readonly string applicationUrl = config.ApplicationUrl;
        private readonly IEmailService emailService = emailService;
        private readonly ILogger<CommandHandler> logger = logger;
        private readonly PidpDbContext context = context;

        public async Task<Model> HandleAsync(Command command)
        {
            this.logger.LogHandlingVerifiedEmailCreate(command.PartyId, command.Email);

            var verifiedEmail = await this.context.VerifiedEmails
                .Where(verifiedEmail => verifiedEmail.PartyId == command.PartyId
#pragma warning disable CA1304, CA1862, CA1311
                    && verifiedEmail.Email.ToLower() == command.Email.ToLower())
#pragma warning restore CA1304, CA1862, CA1311
                .SingleOrDefaultAsync();

            if (verifiedEmail == null)
            {
                this.logger.LogCreatingNewVerifiedEmail(command.PartyId, command.Email);
                verifiedEmail = new VerifiedEmail
                {
                    PartyId = command.PartyId,
                    Token = Guid.NewGuid(),
                    Email = command.Email
                };

                this.context.VerifiedEmails.Add(verifiedEmail);
                await this.context.SaveChangesAsync();
            }
            else
            {
                this.logger.LogVerifiedEmailExists(command.PartyId, verifiedEmail.IsVerified);
            }

            if (!verifiedEmail.IsVerified)
            {
                this.logger.LogSendingVerificationEmail(verifiedEmail.Email);
                await this.SendVerificationEmailAsync(verifiedEmail.Email, verifiedEmail.Token);
            }

            return new Model
            {
                IsVerified = verifiedEmail.IsVerified
            };
        }

        private async Task SendVerificationEmailAsync(string recipientEmail, Guid token)
        {
            var url = $"{this.applicationUrl}/account/external-account".SetQueryParam("email-verification-token", token);
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

public static partial class CreateLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Information, "Handling Verified Email Create for PartyId {PartyId} and Email {Email}")]
    public static partial void LogHandlingVerifiedEmailCreate(this ILogger logger, int partyId, string email);

    [LoggerMessage(2, LogLevel.Information, "VerifiedEmail record already exists for PartyId {PartyId}. IsVerified: {IsVerified}")]
    public static partial void LogVerifiedEmailExists(this ILogger logger, int partyId, bool isVerified);

    [LoggerMessage(3, LogLevel.Information, "Creating new VerifiedEmail record for PartyId {PartyId} and Email {Email}")]
    public static partial void LogCreatingNewVerifiedEmail(this ILogger logger, int partyId, string email);

    [LoggerMessage(4, LogLevel.Information, "Sending verification email to {Email}")]
    public static partial void LogSendingVerificationEmail(this ILogger logger, string email);
}
