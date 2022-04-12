namespace Pidp.Features.AccessRequests;

using DomainResults.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NodaTime;

using Pidp.Data;
using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Infrastructure.HttpClients.Ldap;
using Pidp.Infrastructure.Services;
using Pidp.Models;

public class HcimReEnrolment
{
    public class Command : ICommand<IDomainResult<Model>>
    {
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
        private readonly string hcimClientId;
        private readonly IClock clock;
        private readonly IEmailService emailService;
        private readonly IKeycloakAdministrationClient client;
        private readonly ILdapClient ldapClient;
        private readonly PidpDbContext context;

        public CommandHandler(
            IClock clock,
            IEmailService emailService,
            IKeycloakAdministrationClient client,
            ILdapClient ldapClient,
            PidpConfiguration config,
            PidpDbContext context)
        {
            this.hcimClientId = config.Keycloak.HcimClientId;
            this.clock = clock;
            this.emailService = emailService;
            this.client = client;
            this.ldapClient = ldapClient;
            this.context = context;
        }

        public async Task<IDomainResult<Model>> HandleAsync(Command command)
        {
            var dto = await this.context.Parties
                .Where(party => party.Id == command.PartyId)
                .Select(party => new
                {
                    party.UserId,
                    AlreadyEnroled = party.AccessRequests.Any(request => request.AccessType == AccessType.HcimReEnrolment)
                })
                .SingleAsync(); // Already did existance check

            // TODO check other prerequisites
            if (dto.AlreadyEnroled)
            {
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

            var roleAssignmentSuccess = await this.client.AssignClientRole(dto.UserId, this.hcimClientId, authStatus.HcimUserRole);
            if (!roleAssignmentSuccess)
            {
                return DomainResult.Failed<Model>();
            }

            var newRequest = new HcimReEnrolmentAccessRequest
            {
                PartyId = command.PartyId,
                AccessType = AccessType.HcimReEnrolment,
                RequestedOn = this.clock.GetCurrentInstant(),
                LdapUsername = command.LdapUsername
            };

            this.context.HcimReEnrolmentAccessRequests.Add(newRequest);

            await this.context.SaveChangesAsync();

            // TODO Email?
            // await this.emailService.SendSaEformsAccessRequestConfirmationAsync(command.PartyId);

            return DomainResult.Success(new Model(authStatus));
        }
    }
}
