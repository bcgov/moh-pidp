namespace Pidp.Features.AccessRequests;

using DomainResults.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

using Pidp.Data;
using Pidp.Models;

public class SAeForms
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
        private readonly PidpDbContext context;

        public CommandHandler(PidpDbContext context) => this.context = context;

        public async Task<IDomainResult> HandleAsync(Command command)
        {
            // TODO check standing
            // TODO Keycloak Role
            var existingRequest = await this.context.AccessRequests
                .SingleOrDefaultAsync(request => request.PartyId == command.PartyId
                    && request.AccessType == AccessType.SAeForms);

            if (existingRequest != null)
            {
                return DomainResult.Failed();
            }

            var newRequest = new AccessRequest
            {
                PartyId = command.PartyId,

            };

            this.context.AccessRequests.Add(newRequest);

            await this.context.SaveChangesAsync();

            return DomainResult.Success();
        }
    }
}
