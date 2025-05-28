namespace Pidp.Features.VerifiedEmail;

using FluentValidation;
using HybridModelBinding;
using MassTransit;
using MediatR;
using NodaTime;
using System.Text.Json.Serialization;

using Pidp.Data;
using Pidp.Extensions;
using Pidp.Infrastructure.Auth;
using Pidp.Models;
using Pidp.Models.DomainEvents;

public class Verify
{
    public class Command : ICommand
    {
        [JsonIgnore]
        [HybridBindProperty(Source.Route)]
        public int PartyId { get; set; }
        public Guid Token { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            this.RuleFor(x => x.PartyId).GreaterThan(0);
            this.RuleFor(x => x.Token).NotEmpty();
        }
    }

    public class CommandHandler(PidpDbContext context) : ICommandHandler<Command>
    {
        private readonly PidpDbContext context = context;

        public async Task HandleAsync(Command command)
        {
            // check token
            // raise event, etc.

            return;
        }
    }
}
