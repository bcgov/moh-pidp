namespace Pidp.Features.Endorsements;

using DomainResults.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

using Pidp.Data;

public class Cancel
{
    public class Command : ICommand<IDomainResult>
    {
        public int EndorsementId { get; set; }
        public int PartyId { get; set; }
    }

    public class ComandValidator : AbstractValidator<Command>
    {
        public ComandValidator()
        {
            this.RuleFor(x => x.EndorsementId).GreaterThan(0);
            this.RuleFor(x => x.PartyId).GreaterThan(0);
        }
    }

    public class CommandHandler : ICommandHandler<Command, IDomainResult>
    {
        private readonly PidpDbContext context;

        public CommandHandler(PidpDbContext context) => this.context = context;

        public async Task<IDomainResult> HandleAsync(Command command)
        {
            var endorsement = await this.context.EndorsementRelationships
                .Where(relationship => relationship.EndorsementId == command.EndorsementId
                    && relationship.PartyId == command.PartyId)
                .Select(relationship => relationship.Endorsement)
                .SingleOrDefaultAsync();

            if (endorsement == null)
            {
                return DomainResult.NotFound();
            }

            endorsement.Active = false;

            await this.context.SaveChangesAsync();
            return DomainResult.Success();
        }
    }
}
