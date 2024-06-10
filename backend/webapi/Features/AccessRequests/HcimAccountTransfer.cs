namespace Pidp.Features.AccessRequests;

using DomainResults.Common;
using FluentValidation;
using HybridModelBinding;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using System.Text.Json.Serialization;

using Pidp.Data;
using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Infrastructure.HttpClients.Ldap;
using Pidp.Infrastructure.HttpClients.Mail;
using Pidp.Infrastructure.Services;
using Pidp.Models.Lookups;

public class HcimAccountTransfer
{
    public class Command : ICommand<IDomainResult<Model>>
    {
        [JsonIgnore]
        [HybridBindProperty(Source.Route)]
        public int PartyId { get; set; }
        public string LdapUsername { get; set; } = string.Empty;
        public string LdapPassword { get; set; } = string.Empty;
    }

    public class Model
    {
        public HcimAuthorizationStatus.AuthorizationStatus AuthStatus { get; set; }
        public int? RemainingAttempts { get; set; }

        public Model() { }

        public Model(HcimAuthorizationStatus authStatus)
        {
            this.AuthStatus = authStatus.Status;
            this.RemainingAttempts = authStatus.RemainingAttempts;
        }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            this.RuleFor(x => x.PartyId).GreaterThan(0);
            this.RuleFor(x => x.LdapUsername).NotEmpty();
            this.RuleFor(x => x.LdapPassword).NotEmpty();
        }
    }

    public class CommandHandler : ICommandHandler<Command, IDomainResult<Model>>
    {
        private readonly IClock clock;
        private readonly IEmailService emailService;
        private readonly IKeycloakAdministrationClient keycloakClient;
        private readonly ILdapClient ldapClient;
        private readonly ILogger<CommandHandler> logger;
        private readonly PidpDbContext context;
        private readonly string hcimClientId;

        public CommandHandler(
            IClock clock,
            IEmailService emailService,
            IKeycloakAdministrationClient keycloakClient,
            ILdapClient ldapClient,
            ILogger<CommandHandler> logger,
            PidpConfiguration config,
            PidpDbContext context)
        {
            this.clock = clock;
            this.emailService = emailService;
            this.keycloakClient = keycloakClient;
            this.ldapClient = ldapClient;
            this.logger = logger;
            this.context = context;
            this.hcimClientId = config.Keycloak.HcimClientId;
        }

        public async Task<IDomainResult<Model>> HandleAsync(Command command)
        {
            var dto = await this.context.Parties
                .Where(party => party.Id == command.PartyId)
                .Select(party => new
                {
                    AlreadyEnroled = party.AccessRequests.Any(request => request.AccessTypeCode == AccessTypeCode.HcimAccountTransfer),
                    DemographicsComplete = party.Email != null && party.Phone != null,
                    UserId = party.PrimaryUserId,
                    party.Email
                })
                .SingleAsync();

            if (dto.AlreadyEnroled
                || !dto.DemographicsComplete)
            {
                this.logger.LogAccessRequestDenied();
                return DomainResult.Failed<Model>();
            }

            var loginResult = await this.ldapClient.HcimLoginAsync(command.LdapUsername, command.LdapPassword);
            if (!loginResult.IsSuccess)
            {
                return loginResult.To<Model>();
            }

            var authStatus = loginResult.Value;

            if (!authStatus.IsAuthorized)
            {
                // DomainResult can only pass a value on Success.
                return DomainResult.Success(new Model(authStatus));
            }

            if (!await this.UpdateKeycloakUser(dto.UserId, authStatus.OrgDetails, authStatus.HcimUserRole))
            {
                return DomainResult.Failed<Model>();
            }

            this.context.HcimAccountTransfers.Add(new Models.HcimAccountTransfer
            {
                PartyId = command.PartyId,
                AccessTypeCode = AccessTypeCode.HcimAccountTransfer,
                RequestedOn = this.clock.GetCurrentInstant(),
                LdapUsername = command.LdapUsername
            });

            await this.context.SaveChangesAsync();

            await this.SendConfirmationEmail(dto.Email!);

            return DomainResult.Success(new Model(authStatus));
        }

        private async Task<bool> UpdateKeycloakUser(Guid userId, LdapLoginResponse.OrgDetails orgDetails, string hcimRole)
        {
            if (!await this.keycloakClient.UpdateUser(userId, (user) => user.SetLdapOrgDetails(orgDetails)))
            {
                return false;
            }

            if (!await this.keycloakClient.AssignClientRole(userId, this.hcimClientId, hcimRole))
            {
                return false;
            }

            return true;
        }

        private async Task SendConfirmationEmail(string partyEmail)
        {
            var link = $"<a href=\"https://hcimweb-cl.hlth.gov.bc.ca/\" target=\"_blank\" rel=\"noopener noreferrer\">this link</a>";
            var email = new Email(
                from: EmailService.PidpEmail,
                to: partyEmail,
                subject: "HCIMWeb Account Transfer Confirmation",
                body: $"You have successfully transferred your HCIMWeb Account. From now on, use {link} to login to HCIMWeb with your organization credential. You may wish to bookmark this link for future use."
            );

            await this.emailService.SendAsync(email);
        }
    }
}

public static partial class HcimAccountTransferLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Warning, "HCIM Account Transfer Request denied due to the Party Record not meeting all prerequisites.")]
    public static partial void LogAccessRequestDenied(this ILogger<HcimAccountTransfer.CommandHandler> logger);
}
