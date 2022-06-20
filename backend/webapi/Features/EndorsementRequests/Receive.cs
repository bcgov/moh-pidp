namespace Pidp.Features.EndorsementRequests;

using DomainResults.Common;
using FluentValidation;
using HybridModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

using Pidp.Data;

public class Receive
{
    public class Command : ICommand<IDomainResult>
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

    public class CommandHandler : ICommandHandler<Command, IDomainResult>
    {
        private readonly ILogger logger;
        private readonly PidpDbContext context;

        public CommandHandler(ILogger<CommandHandler> logger, PidpDbContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public async Task<IDomainResult> HandleAsync(Command command)
        {
            var receivedRequest = await this.context.EndorsementRequests
                .Where(request => request.Token == command.Token)
                .SingleOrDefaultAsync();

            if (receivedRequest == null)
            {
                return DomainResult.NotFound();
            }
            if (receivedRequest.RequestingPartyId == command.PartyId)
            {
                this.logger.LogSelfEndorsementAttempt(command.PartyId);
                return DomainResult.Failed();
            }
            if (receivedRequest.EndorsingPartyId.HasValue)
            {
                // Already received
                return DomainResult.Failed();
            }

            receivedRequest.EndorsingPartyId = command.PartyId;

            await this.context.SaveChangesAsync();

            return DomainResult.Success();
        }
    }
}

public static partial class EndorsementRequestReceiveLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Warning, "Possible fraudulent behaviour: Party {partyId} attempted to endorse themselves.")]
    public static partial void LogSelfEndorsementAttempt(this ILogger logger, int partyId);
}
