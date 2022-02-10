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

public class SAEforms
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
        private readonly IKeycloakAdministrationClient client;
        private readonly PidpDbContext context;
        private readonly IEmailService emailService;

        public CommandHandler(
            IClock clock,
            IKeycloakAdministrationClient client,
            PidpDbContext context,
            IEmailService emailService)
        {
            this.clock = clock;
            this.client = client;
            this.context = context;
            this.emailService = emailService;
        }

        public async Task<IDomainResult> HandleAsync(Command command)
        {
            // TODO check standing
            var dto = await this.context.Parties
                .Where(party => party.Id == command.PartyId)
                .Select(party => new
                {
                    party.UserId,
                    AccessTypes = party.AccessRequests.Select(x => x.AccessType),
                })
                .SingleOrDefaultAsync();

            if (dto == null)
            {
                return DomainResult.NotFound();
            }

            if (dto.AccessTypes.Contains(AccessType.SAEforms))
            {
                return DomainResult.Failed();
            }

            var newRequest = new AccessRequest
            {
                PartyId = command.PartyId,
                AccessType = AccessType.SAEforms,
                RequestedOn = this.clock.GetCurrentInstant()
            };

            this.context.AccessRequests.Add(newRequest);

            await this.context.SaveChangesAsync();

            // TODO what happens if the role assignment fails?
            await this.client.AssignClientRole(dto.UserId, Resources.SAEforms, Roles.SAEforms);

            await this.emailService.SendSaEformsAccessRequestConfirmationAsync(command.PartyId);

            return DomainResult.Success();
        }
    }
}
