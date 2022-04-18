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
            var dto = await this.context.Parties
                .Where(party => party.Id == command.PartyId)
                .Select(party => new
                {
                    party.UserId,
                    AlreadyEnroled = party.AccessRequests.Any(request => request.AccessType == AccessType.SAEforms),
                })
                .SingleAsync();

            // TODO check standing
            if (dto.AlreadyEnroled)
            {
                return DomainResult.Failed();
            }

            if (!await this.client.AssignClientRole(dto.UserId, Resources.SAEforms, Roles.SAEforms))
            {
                return DomainResult.Failed();
            }

            this.context.AccessRequests.Add(new AccessRequest
            {
                PartyId = command.PartyId,
                AccessType = AccessType.SAEforms,
                RequestedOn = this.clock.GetCurrentInstant()
            });

            await this.context.SaveChangesAsync();

            await this.emailService.SendSaEformsAccessRequestConfirmationAsync(command.PartyId);

            return DomainResult.Success();
        }
    }
}
