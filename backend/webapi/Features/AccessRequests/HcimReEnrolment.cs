namespace Pidp.Features.AccessRequests;

using DomainResults.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NodaTime;

using Pidp.Data;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Infrastructure.Services;
using Pidp.Models;

public class HcimReEnrolment
{
    public class Command : ICommand<IDomainResult>
    {
        public int PartyId { get; set; }
        public string LdapUsername { get; set; } = string.Empty;
        public string LdapPassword { get; set; } = string.Empty;
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

    public class CommandHandler : ICommandHandler<Command, IDomainResult>
    {
        private readonly IClock clock;
        private readonly IEmailService emailService;
        private readonly IKeycloakAdministrationClient client;
        private readonly PidpDbContext context;

        public CommandHandler(
            IClock clock,
            IEmailService emailService,
            IKeycloakAdministrationClient client,
            PidpDbContext context)
        {
            this.clock = clock;
            this.emailService = emailService;
            this.client = client;
            this.context = context;
        }

        public async Task<IDomainResult> HandleAsync(Command command)
        {
            // TODO check prerequisites
            // TODO additional LDAP properties
            var newRequest = new AccessRequest
            {
                PartyId = command.PartyId,
                AccessType = AccessType.HcimReEnrolment,
                RequestedOn = this.clock.GetCurrentInstant()
            };

            this.context.AccessRequests.Add(newRequest);

            await this.context.SaveChangesAsync();

            // TODO role assignment?
            // await this.client.AssignClientRole(dto.UserId, Resources.SAEforms, Roles.SAEforms);

            // TODO Email?
            // await this.emailService.SendSaEformsAccessRequestConfirmationAsync(command.PartyId);

            return DomainResult.Success();
        }
    }
}
