namespace Pidp.Features.EndorsementRequests;

using DomainResults.Common;
using FluentValidation;
using HybridModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

using Pidp.Data;
using Pidp.Models;
using NodaTime;

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
        private readonly IClock clock;
        private readonly ILogger<CommandHandler> logger;
        private readonly PidpDbContext context;

        public CommandHandler(
            IClock clock,
            ILogger<CommandHandler> logger,
            PidpDbContext context)
        {
            this.clock = clock;
            this.logger = logger;
            this.context = context;
        }

        public async Task<IDomainResult> HandleAsync(Command command)
        {
            var endorsementRequest = await this.context.EndorsementRequests
                .SingleOrDefaultAsync(request => request.Token == command.Token);

            if (endorsementRequest == null)
            {
                return DomainResult.NotFound();
            }
            if (endorsementRequest.Status != EndorsementRequestStatus.Created)
            {
                // Already received
                return DomainResult.Failed();
            }
            if (endorsementRequest.RequestingPartyId == command.PartyId)
            {
                this.logger.LogSelfEndorsementAttempt(command.PartyId);
                return DomainResult.Failed();
            }

            endorsementRequest.ReceivingPartyId = command.PartyId;
            endorsementRequest.Status = EndorsementRequestStatus.Received;
            endorsementRequest.StatusDate = this.clock.GetCurrentInstant();

            await this.context.SaveChangesAsync();

            return DomainResult.Success();
        }
    }
}

public static partial class EndorsementRequestReceiveLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Warning, "Possible fraudulent behaviour: Party {partyId} received an Endorsement Request from themselves.")]
    public static partial void LogSelfEndorsementAttempt(this ILogger<Receive.CommandHandler> logger, int partyId);
}
