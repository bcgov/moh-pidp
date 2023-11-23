namespace Pidp.Features.EndorsementRequests;

using DomainResults.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NodaTime;

using Pidp.Data;
using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Infrastructure.HttpClients.Mail;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Infrastructure.Services;
using Pidp.Models;
using Pidp.Models.DomainEvents;

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
        private readonly IEmailService emailService;
        private readonly IKeycloakAdministrationClient keycloakClient;
        private readonly ILogger logger;
        private readonly IPlrClient plrClient;
        private readonly PidpDbContext context;

        public CommandHandler(
            IClock clock,
            IEmailService emailService,
            IKeycloakAdministrationClient keycloakClient,
            ILogger<CommandHandler> logger,
            IPlrClient plrClient,
            PidpDbContext context)
        {
            this.clock = clock;
            this.emailService = emailService;
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
                this.context.Endorsements.Add(Endorsement.FromCompletedRequest(endorsementRequest));
                await this.context.SaveChangesAsync(); // This double Save is deliberate; we need to persist the Endorsement Relationships in the database before we can calculate the EndorserData in the Domain Events.
                await this.HandleMoaUpdates(endorsementRequest);

                await this.SendEndorsementApprovedEmailAsync(endorsementRequest);
            }

            await this.context.SaveChangesAsync();

            return DomainResult.Success();
        }

        /// <summary>
        /// When a user with no College Licences is endorsed by a Licenced user in good standing, they recieve the "MOA" role in Keycloak.
        /// Also, the BC Provider for one or both Parties might need updating.
        /// </summary>
        private async Task HandleMoaUpdates(EndorsementRequest request)
        {
            // TODO: We don't actually need the whole Party, just the Id, PrimaryUserId and Cpn.
            // Consider a generic way of attaching domain events so we don't have to fetch the entire model.
            var parties = await this.context.Parties
                .Include(party => party.Credentials)
                .Where(party => party.Id == request.RequestingPartyId
                    || party.Id == request.ReceivingPartyId)
                .ToListAsync();

            var party1 = parties[0];
            var party2 = parties[1];

            if (await this.plrClient.GetStandingAsync(party1.Cpn))
            {
                party2.DomainEvents.Add(new EndorsementStandingUpdated(party2.Id));

                if (string.IsNullOrEmpty(party2.Cpn))
                {
                    await this.AssignMoaRole(party2);
                }
            }

            if (await this.plrClient.GetStandingAsync(party2.Cpn))
            {
                party1.DomainEvents.Add(new EndorsementStandingUpdated(party1.Id));

                if (string.IsNullOrEmpty(party1.Cpn))
                {
                    await this.AssignMoaRole(party1);
                }
            }
        }

        private async Task AssignMoaRole(Party party)
        {
            if (await this.keycloakClient.AssignAccessRoles(party.PrimaryUserId, MohKeycloakEnrolment.MoaLicenceStatus))
            {
                this.context.BusinessEvents.Add(LicenceStatusRoleAssigned.Create(party.Id, MohKeycloakEnrolment.MoaLicenceStatus, this.clock.GetCurrentInstant()));
            }
            else
            {
                this.logger.LogMoaRoleAssignmentError(party.Id);
            }
        }

        private async Task SendEndorsementApprovedEmailAsync(EndorsementRequest request)
        {
            var receivingPartyEmail = await this.context.Parties
                                        .Where(party => party.Id == request.RequestingPartyId)
                                        .Select(party => party.Email)
                                        .SingleAsync();
            var applicationUrl = $"<a href=\"{this.config.ApplicationUrl}\" target=\"_blank\" rel=\"noopener noreferrer\">OneHealthID Service</a>";
            var pidpSupportEmail = $"<a href=\"mailto:{EmailService.PidpEmail}\">{EmailService.PidpEmail}</a>";
            var pidpSupportPhone = $"<a href=\"tel:{EmailService.PidpSupportPhone}\">{EmailService.PidpSupportPhone}</a>";

            var email = new Email(
                from: EmailService.PidpEmail,
                to: receivingPartyEmail!,
                subject: $"OneHealthID Endorsement is approved",
                body: $@"Hello,
<br>You are receiving this email because your endorsement is now complete in the {applicationUrl}. You should now be able to access and use the Provincial Attachment System. You can now continue enrolling in any services that required endorsement from a licensed provider.
<br>
<br>For additional support, contact the OneHealthID Service desk:
<br>
<br>&emsp;• By email at {pidpSupportEmail}
<br>
<br>&emsp;• By phone at {pidpSupportPhone}
<br>
<br>Thank you.");

            await this.emailService.SendAsync(email);
        }
    }
}

public static partial class EndorsementRequestApprovalLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Error, "Error when assigning the MOA role to Party #{partyId}.")]
    public static partial void LogMoaRoleAssignmentError(this ILogger logger, int partyId);
}
