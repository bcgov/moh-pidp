namespace Pidp.Features.AccessRequests;

using DomainResults.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

using Pidp.Infrastructure.HttpClients.Plr;

public class ImmsBC
{
    public static IdentifierType[] AllowedIdentifierTypes => [IdentifierType.PhysiciansAndSurgeons, IdentifierType.Nurse, IdentifierType.Midwife];

    public class Command : ICommand<IDomainResult>
    {
        public int PartyId { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator() => this.RuleFor(x => x.PartyId).GreaterThan(0);
    }

    public class CommandHandler() : ICommandHandler<Command, IDomainResult>
    {


        public async Task<IDomainResult> HandleAsync(Command command) => DomainResult.Success();

    }
}

