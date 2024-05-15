namespace Pidp.Features.AccessRequests;

using DomainResults.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NodaTime;

using Pidp.Data;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.BCProvider;
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
        private readonly string bcProviderClientId;
        private readonly IBCProviderClient client;
        private readonly IClock clock;
        private readonly ILogger<CommandHandler> logger;
        private readonly PidpDbContext context;

        public CommandHandler(
            IBCProviderClient client,
            IClock clock,
            ILogger<CommandHandler> logger,
            PidpDbContext context,
            PidpConfiguration config)
        {
            this.bcProviderClientId = config.BCProviderClient.ClientId;
            this.client = client;
            this.clock = clock;
            this.logger = logger;
            this.context = context;
        }

        public async Task<IDomainResult> HandleAsync(Command command)
        {
            var party = await this.context.Parties
                .Where(party => party.Id == command.PartyId)
                .Select(party => new
                {
                    AlreadyEnrolled = party.AccessRequests.Any(request => request.AccessTypeCode == AccessTypeCode.UserAccessAgreement),
                    UserPrincipalName = party.Credentials
                        .Where(credential => credential.IdentityProvider == IdentityProviders.BCProvider)
                        .Select(credential => credential.IdpId)
                        .SingleOrDefault()
                })
                .SingleAsync();

            if (party.AlreadyEnrolled)
            {
                this.logger.LogAgreementHasAlreadyBeenAccepted(command.PartyId);
                return DomainResult.Failed();
            }

            var timestamp = this.clock.GetCurrentInstant();

            this.context.AccessRequests.Add(new AccessRequest
            {
                PartyId = command.PartyId,
                AccessTypeCode = AccessTypeCode.UserAccessAgreement,
                RequestedOn = timestamp
            });

            await this.context.SaveChangesAsync();

            // BC Provider registration requires the User Access Agreeement.
            // This update is only for the users that aquired BC Provider accounts before UAA was added to the app.
            // One day, this can be removed entireley.
            if (!string.IsNullOrWhiteSpace(party.UserPrincipalName))
            {
                var attributes = new BCProviderAttributes(this.bcProviderClientId)
                    .SetUaaDate(timestamp.ToDateTimeOffset());

                await this.client.UpdateAttributes(party.UserPrincipalName, attributes.AsAdditionalData());
            }

            return DomainResult.Success();
        }
    }
}

public static partial class UserAccessAgreementLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Warning, "User {partyId} has already accepted the user access agreement.")]
    public static partial void LogAgreementHasAlreadyBeenAccepted(this ILogger<UserAccessAgreement.CommandHandler> logger, int partyId);
}
