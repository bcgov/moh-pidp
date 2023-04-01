namespace Pidp.Features.EndorsementRequests;

using DomainResults.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NodaTime;

using Pidp.Data;
using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models;

public class Approve
{
    public class Command : ICommand<IDomainResult>
    {
        public int PartyId { get; set; }
        public int EndorsementRequestId { get; set; }
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
        private readonly IKeycloakAdministrationClient keycloakClient;
        private readonly ILogger logger;
        private readonly IPlrClient plrClient;
        private readonly PidpDbContext context;

        public CommandHandler(
            IClock clock,
            IKeycloakAdministrationClient keycloakClient,
            ILogger<CommandHandler> logger,
            IPlrClient plrClient,
            PidpDbContext context)
        {
            this.clock = clock;
            this.keycloakClient = keycloakClient;
            this.logger = logger;
            this.plrClient = plrClient;
            this.context = context;
        }

        public async Task<IDomainResult> HandleAsync(Command command)
        {
            var endorsementRequest = await this.context.EndorsementRequests
                .SingleOrDefaultAsync(request => request.Id == command.EndorsementRequestId);

            if (endorsementRequest == null)
            {
                return DomainResult.NotFound();
            }

            var result = endorsementRequest.Handle(command, this.clock);
            if (!result.IsSuccess)
            {
                return result;
            }

            if (endorsementRequest.Status == EndorsementRequestStatus.Completed)
            {
                // Both parties have approved, Request handshake is complete.
                await this.HandleMoaDesignation(endorsementRequest);
                this.context.Endorsements.Add(Endorsement.FromCompletedRequest(endorsementRequest));
            }

            await this.context.SaveChangesAsync();

            return DomainResult.Success();
        }

        // When a user with no College Licences is endorsed by a Licenced user in good standing, they recieve the "MOA" role.
        private async Task HandleMoaDesignation(EndorsementRequest request)
        {
            var parties = await this.context.Parties
                .Where(party => party.Id == request.RequestingPartyId
                    || party.Id == request.ReceivingPartyId)
                .Select(party => new
                {
                    party.Id,
                    UserId = party.PrimaryUserId,
                    party.Cpn
                })
                .ToListAsync();

            if (parties.Count(party => string.IsNullOrWhiteSpace(party.Cpn)) != 1)
            {
                // Either both Parties are licenced or both Parties are unlicenced
                return;
            }

            var licencedParty = parties.Single(party => !string.IsNullOrWhiteSpace(party.Cpn));
            if (!await this.plrClient.GetStandingAsync(licencedParty.Cpn))
            {
                // TODO: Log / other behaviour when in bad standing?
                return;
            }

            var unLicencedParty = parties.Single(party => string.IsNullOrWhiteSpace(party.Cpn));
            if (await this.keycloakClient.AssignAccessRoles(unLicencedParty.UserId, MohKeycloakEnrolment.MoaLicenceStatus))
            {
                this.context.BusinessEvents.Add(LicenceStatusRoleAssigned.Create(unLicencedParty.Id, MohKeycloakEnrolment.MoaLicenceStatus, this.clock.GetCurrentInstant()));
            }
            else
            {
                this.logger.LogMoaRoleAssignmentError(unLicencedParty.Id);
            };
        }
    }
}

public static partial class EndorsementRequestApprovalLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Error, "Error when assigning the MOA role to Party #{partyId}.")]
    public static partial void LogMoaRoleAssignmentError(this ILogger logger, int partyId);
}
