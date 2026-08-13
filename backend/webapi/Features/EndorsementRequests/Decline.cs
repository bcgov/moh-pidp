namespace Pidp.Features.EndorsementRequests;

using DomainResults.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NodaTime;

using Pidp.Data;
using Pidp.Models;

public class Decline
{
    public class Command : ICommand<IDomainResult>
    {
        public int PartyId { get; set; }
        public int EndorsementRequestId { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            this.RuleFor(x => x.PartyId).GreaterThan(0);
            this.RuleFor(x => x.EndorsementRequestId).GreaterThan(0);
        }
    }

    public class CommandHandler(IClock clock, PidpDbContext context) : ICommandHandler<Command, IDomainResult>
    {
        private readonly IClock clock = clock;
        private readonly PidpDbContext context = context;

        public async Task<IDomainResult> HandleAsync(Command command)
        {
            var endorsementRequest = await this.context.EndorsementRequests
                .SingleOrDefaultAsync(request => request.Id == command.EndorsementRequestId);

            if (endorsementRequest == null)
            {
                return DomainResult.NotFound();
            }

            var result = endorsementRequest.Handle(command, this.clock);
            if (!result.IsSuccess)
            {
                return result;
            }

            this.context.BusinessEvents.Add(EndorsementDenied.Create(command.PartyId, command.EndorsementRequestId, this.clock.GetCurrentInstant()));
            await this.context.SaveChangesAsync();

            return DomainResult.Success();
        }
    }
}
