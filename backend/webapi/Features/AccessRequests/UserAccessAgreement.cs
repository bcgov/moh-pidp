namespace Pidp.Features.AccessRequests;

using DomainResults.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NodaTime;

using Pidp.Data;
using Pidp.Models;
using Pidp.Models.Lookups;

public class UserAccessAgreement
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
        private readonly ILogger logger;
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
            var alreadyEnroled = await this.context.Parties
                .Where(party => party.Id == command.PartyId)
                .Select(party =>
                    party.AccessRequests.Any(request
                        => request.AccessTypeCode == AccessTypeCode.UserAccessAgreement))
                .SingleAsync();

            if (alreadyEnroled)
            {
                this.logger.LogUserAccessAgreementHasAlreadyBeenAccepted();
                return DomainResult.Failed();
            }

            this.context.AccessRequests.Add(new AccessRequest
            {
                PartyId = command.PartyId,
                AccessTypeCode = AccessTypeCode.UserAccessAgreement,
                RequestedOn = this.clock.GetCurrentInstant()
            });

            await this.context.SaveChangesAsync();

            return DomainResult.Success();
        }
    }
}

public static partial class UserAccessAgreementLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Warning, "The user access agreement has already been accepted.")]
    public static partial void LogUserAccessAgreementHasAlreadyBeenAccepted(this ILogger logger);
}
