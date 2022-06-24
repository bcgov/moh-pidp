namespace Pidp.Features.EndorsementRequests;

using DomainResults.Common;
using FluentValidation;
using HybridModelBinding;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using System.Text.Json.Serialization;

using Pidp.Data;

public class Adjudicate
{
    public class Command : ICommand<IDomainResult>
    {
        [JsonIgnore]
        [HybridBindProperty(Source.Route)]
        public int PartyId { get; set; }
        [JsonIgnore]
        [HybridBindProperty(Source.Route)]
        public int EndorsementRequestId { get; set; }
        public bool Approved { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            this.RuleFor(x => x.PartyId).GreaterThan(0);
            this.RuleFor(x => x.EndorsementRequestId).GreaterThan(0);
        }
    }

    public class CommandHandler : ICommandHandler<Command, IDomainResult>
    {
        private readonly IClock clock;
        private readonly PidpDbContext context;

        public CommandHandler(IClock clock, PidpDbContext context)
        {
            this.clock = clock;
            this.context = context;
        }

        public async Task<IDomainResult> HandleAsync(Command command)
        {
            var endorsementRequest = await this.context.EndorsementRequests
                .SingleOrDefaultAsync(request => request.Id == command.EndorsementRequestId
                    && request.EndorsingPartyId == command.PartyId);

            if (endorsementRequest == null)
            {
                return DomainResult.NotFound();
            }
            if (endorsementRequest.Approved.HasValue)
            {
                return DomainResult.Failed();
            }

            endorsementRequest.Approved = command.Approved;
            endorsementRequest.AdjudicatedOn = this.clock.GetCurrentInstant();

            await this.context.SaveChangesAsync();

            return DomainResult.Success();
        }
    }
}
