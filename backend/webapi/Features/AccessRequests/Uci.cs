namespace Pidp.Features.AccessRequests;

using DomainResults.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NodaTime;

using Pidp.Data;
using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Infrastructure.HttpClients.Mail;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Infrastructure.Services;
using Pidp.Models;
using Pidp.Models.Lookups;

public class Uci
{
    public class Command : ICommand<IDomainResult>
    {
        public int PartyId { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator() => this.RuleFor(x => x.PartyId).GreaterThan(0);
    }

    public class CommandHandler : ICommandHandler<Command, IDomainResult>
    {
        private readonly IClock clock;
        private readonly IEmailService emailService;
        private readonly IKeycloakAdministrationClient keycloakClient;
        private readonly ILogger logger;
        private readonly IPlrClient plrClient;
        private readonly PidpDbContext context;

        public CommandHandler(
            IClock clock,
            IEmailService emailService,
            IKeycloakAdministrationClient keycloakClient,
            ILogger<CommandHandler> logger,
            IPlrClient plrClient,
            PidpDbContext context)
        {
            this.clock = clock;
            this.emailService = emailService;
            this.keycloakClient = keycloakClient;
            this.logger = logger;
            this.plrClient = plrClient;
            this.context = context;
        }

        public async Task<IDomainResult> HandleAsync(Command command)
        {
            var dto = await this.context.Parties
                .Where(party => party.Id == command.PartyId)
                .Select(party => new
                {
                    AlreadyEnroled = party.AccessRequests.Any(request => request.AccessTypeCode == AccessTypeCode.Uci),
                    party.UserId,
                    party.Email,
                    party.Cpn
                })
                .SingleAsync();

            if (dto.AlreadyEnroled
                || dto.Email == null
                || !await this.plrClient.GetStandingAsync(dto.Cpn))
            {
                this.logger.LogSAEformsAccessRequestDenied();
                return DomainResult.Failed();
            }

            if (!await this.keycloakClient.AssignClientRole(dto.UserId, MohClients.Uci.ClientId, MohClients.Uci.AccessRole))
            {
                return DomainResult.Failed();
            }

            this.context.AccessRequests.Add(new AccessRequest
            {
                PartyId = command.PartyId,
                AccessTypeCode = AccessTypeCode.Uci,
                RequestedOn = this.clock.GetCurrentInstant()
            });

            await this.context.SaveChangesAsync();

            await this.SendConfirmationEmailAsync(dto.Email);

            return DomainResult.Success();
        }

        private async Task SendConfirmationEmailAsync(string partyEmail)
        {
            var link = $"<a href=\"https://uci-saml.fraserhealth.org/ExSSOIdentityProvider/Account/Login?ReturnUrl=%2fExSSOIdentityProvider%2fExternalLogin.aspx%3fclientApp%3dex_webAccess&clientApp=ex_webAccess\" target=\"_blank\" rel=\"noopener noreferrer\">link</a>";
            var email = new Email(
                from: EmailService.PidpEmail,
                to: partyEmail,
                subject: "UCI Enrolment Confirmation",
                body: $"You have successfully enrolled for UCI. You may wish to bookmark this {link} for future use."
            );
            await this.emailService.SendAsync(email);
        }
    }
}

public static partial class UciLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Warning, "UCI Access Request denied due to the Party Record not meeting all prerequisites.")]
    public static partial void LogUciAccessRequestDenied(this ILogger logger);
}
