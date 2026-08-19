namespace Pidp.Features.AccessRequests;

using DomainResults.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NodaTime;

using Pidp.Data;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models;
using Pidp.Models.Lookups;

public class HcimWebPcr
{
    public class Command : ICommand<IDomainResult>
    {
        public required int PartyId { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator() => this.RuleFor(x => x.PartyId).GreaterThan(0);
    }

    public class CommandHandler(
        IClock clock,
        IKeycloakAdministrationClient keycloakClient,
        ILogger<CommandHandler> logger,
        IPlrClient plrClient,
        PidpDbContext context) : ICommandHandler<Command, IDomainResult>
    {
        private readonly IClock clock = clock;
        private readonly IKeycloakAdministrationClient keycloakClient = keycloakClient;
        private readonly ILogger<CommandHandler> logger = logger;
        private readonly IPlrClient plrClient = plrClient;
        private readonly PidpDbContext context = context;

        public async Task<IDomainResult> HandleAsync(Command command)
        {
            var dto = await this.context.Parties
                .Where(party => party.Id == command.PartyId)
                .Select(party => new
                {
                    AlreadyEnroled = party.AccessRequests.Any(request => request.AccessTypeCode == AccessTypeCode.HcimWebPcr),
                    HasBCServicesCardCredential = party.Credentials.Any(credential => credential.IdentityProvider == IdentityProviders.BCServicesCard),
                    UserId = party.Credentials
                        .Where(credential => credential.IdentityProvider == IdentityProviders.BCProvider)
                        .Select(credential => (Guid?)credential.UserId)
                        .FirstOrDefault(),
                    party.Cpn,
                })
                .SingleAsync();

            if (dto.AlreadyEnroled
                || !dto.HasBCServicesCardCredential
                || dto.UserId == null
                || !(await this.plrClient.GetStandingsDigestAsync(dto.Cpn)).HasGoodStanding)
            {
                this.logger.LogAccessRequestDenied();

                this.context.BusinessEvents.Add(AccessRequestFailed.Create(command.PartyId, AccessTypeCode.HcimWebPcr.ToString(), this.clock.GetCurrentInstant()));
                await this.context.SaveChangesAsync();

                return DomainResult.Failed();
            }

            if (!await this.keycloakClient.AssignAccessRoles(dto.UserId.Value, MohKeycloakEnrolment.HcimWebPcr))
            {
                this.logger.LogKeycloakRoleAssignmentFailed(command.PartyId);
                return DomainResult.Failed();
            }

            this.context.AccessRequests.Add(new AccessRequest
            {
                PartyId = command.PartyId,
                AccessTypeCode = AccessTypeCode.HcimWebPcr,
                RequestedOn = this.clock.GetCurrentInstant()
            });

            this.context.BusinessEvents.Add(AccessRequestSubmitted.Create(command.PartyId, AccessTypeCode.HcimWebPcr.ToString(), this.clock.GetCurrentInstant()));

            await this.context.SaveChangesAsync();

            return DomainResult.Success();
        }
    }
}

public static partial class HcimWebPcrLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Warning, "Provincial Client Registry Access Request denied due to the Party Record not meeting all prerequisites.")]
    public static partial void LogAccessRequestDenied(this ILogger<HcimWebPcr.CommandHandler> logger);

    [LoggerMessage(2, LogLevel.Error, "Provincial Client Registry Access Request failed; could not assign the Keycloak Access Roles for Party #{partyId}.")]
    public static partial void LogKeycloakRoleAssignmentFailed(this ILogger<HcimWebPcr.CommandHandler> logger, int partyId);
}
