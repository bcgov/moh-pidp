namespace Pidp.Features.EndorsementRequests;

using DomainResults.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NodaTime;

using Pidp.Data;
using Pidp.Extensions;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.BCProvider;
using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Infrastructure.HttpClients.Mail;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Infrastructure.Services;
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
        private readonly IBCProviderClient bcProviderClient;
        private readonly IClock clock;
        private readonly IEmailService emailService;
        private readonly IKeycloakAdministrationClient keycloakClient;
        private readonly ILogger logger;
        private readonly IPlrClient plrClient;
        private readonly PidpConfiguration config;
        private readonly PidpDbContext context;

        public CommandHandler(
            IBCProviderClient bcProviderClient,
            IClock clock,
            IEmailService emailService,
            IKeycloakAdministrationClient keycloakClient,
            ILogger<CommandHandler> logger,
            IPlrClient plrClient,
            PidpConfiguration config,
            PidpDbContext context)
        {
            this.bcProviderClient = bcProviderClient;
            this.clock = clock;
            this.emailService = emailService;
            this.keycloakClient = keycloakClient;
            this.logger = logger;
            this.plrClient = plrClient;
            this.config = config;
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

                await this.SendEndorsementApprovedEmailAsync(endorsementRequest);
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
                    party.Cpn,
                    UserPrincipalName = party.Credentials
                        .Where(credential => credential.IdentityProvider == IdentityProviders.BCProvider)
                        .Select(credential => credential.IdpId)
                        .SingleOrDefault(),
                })
                .ToListAsync();

            if (parties.Count(party => string.IsNullOrWhiteSpace(party.Cpn)) != 1)
            {
                // Either both Parties are licenced or both Parties are unlicenced
                return;
            }

            var licencedParty = parties.Single(party => !string.IsNullOrWhiteSpace(party.Cpn));
            var licencedPartyStanding = await this.plrClient.GetStandingsDigestAsync(licencedParty.Cpn);
            if (!licencedPartyStanding.HasGoodStanding)
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
            }

            if (!string.IsNullOrWhiteSpace(unLicencedParty.UserPrincipalName)
                && licencedPartyStanding
                    .With(IdentifierType.PhysiciansAndSurgeons, IdentifierType.Nurse, IdentifierType.Midwife)
                    .HasGoodStanding)
            {
                var endorsingCpns = await this.context.ActiveEndorsementRelationships(unLicencedParty.Id)
                    .Select(relationship => relationship.Party!.Cpn)
                    .ToListAsync();

                var endorsingPartiesStanding = await this.plrClient.GetAggregateStandingsDigestAsync(endorsingCpns);

                var unlicencedPartyBCProviderAttributes = new BCProviderAttributes(this.config.BCProviderClient.ClientId)
                    .SetIsMoa(true)
                    .SetEndorserData(endorsingPartiesStanding
                        .WithGoodStanding()
                        .With(IdentifierType.PhysiciansAndSurgeons, IdentifierType.Nurse, IdentifierType.Midwife)
                        .Cpns
                        .Append(licencedParty.Cpn!));
                await this.bcProviderClient.UpdateAttributes(unLicencedParty.UserPrincipalName, unlicencedPartyBCProviderAttributes.AsAdditionalData());
            }
        }

        private async Task SendEndorsementApprovedEmailAsync(EndorsementRequest request)
        {
            var requestingPartyFullName = await this.context.Parties
                                            .Where(party => party.Id == request.RequestingPartyId)
                                            .Select(party => party.FullName)
                                            .SingleAsync();

            var receivingPartyEmail = await this.context.Parties
                                        .Where(party => party.Id == request.RequestingPartyId)
                                        .Select(party => party.Email)
                                        .SingleAsync();

            var email = new Email(
                from: EmailService.PidpEmail,
                to: receivingPartyEmail!,
                subject: $"OneHealthID Endorsement is approved",
                body: $@"The endorsement is approved and the relationship is established with {requestingPartyFullName}");

            await this.emailService.SendAsync(email);
        }
    }
}

public static partial class EndorsementRequestApprovalLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Error, "Error when assigning the MOA role to Party #{partyId}.")]
    public static partial void LogMoaRoleAssignmentError(this ILogger logger, int partyId);
}
