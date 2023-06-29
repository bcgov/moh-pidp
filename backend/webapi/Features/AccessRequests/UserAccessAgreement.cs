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
            var alreadyEnrolled = await this.context.AccessRequests
                .AnyAsync(request => request.PartyId == command.PartyId
                    && request.AccessTypeCode == AccessTypeCode.UserAccessAgreement);

            if (alreadyEnrolled)
            {
                this.logger.LogUserAccessAgreementHasAlreadyBeenAccepted(command.PartyId);
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
    [LoggerMessage(1, LogLevel.Warning, "User {partyId} has already accepted the user access agreement.")]
    public static partial void LogUserAccessAgreementHasAlreadyBeenAccepted(this ILogger logger, int partyId);
}
