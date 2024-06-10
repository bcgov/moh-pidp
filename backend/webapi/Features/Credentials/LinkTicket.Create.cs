namespace Pidp.Features.Credentials;

using DomainResults.Common;
using FluentValidation;
using HybridModelBinding;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using System.Text.Json.Serialization;

using Pidp.Data;
using Pidp.Infrastructure.Auth;
using Pidp.Models;

public class LinkTicketCreate
{
    public class Command : ICommand<IDomainResult<CredentialLinkTicket>>
    {
        [JsonIgnore]
        [HybridBindProperty(Source.Route)]
        public int PartyId { get; set; }
        public string LinkToIdp { get; set; } = string.Empty;
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            var allowedIdps = new[] { IdentityProviders.BCProvider, IdentityProviders.BCServicesCard, IdentityProviders.Phsa };

            this.RuleFor(x => x.PartyId).GreaterThan(0);
            this.RuleFor(x => x.LinkToIdp).NotEmpty()
                .Must(x => allowedIdps.Contains(x)).WithMessage($"Only the following IDPs are supported: {string.Join(", ", allowedIdps)}.");
        }
    }

    public class CommandHandler : ICommandHandler<Command, IDomainResult<CredentialLinkTicket>>
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

        public async Task<IDomainResult<CredentialLinkTicket>> HandleAsync(Command command)
        {
            var idps = await this.context.Credentials
                .Where(credential => credential.PartyId == command.PartyId)
                .Select(credential => credential.IdentityProvider)
                .ToListAsync();

            if (idps.Contains(IdentityProviders.Idir))
            {
                return DomainResult.Failed<CredentialLinkTicket>("IDIR accounts cannot link Credentials.");
            }

            if (!idps.Contains(IdentityProviders.BCServicesCard)
                && command.LinkToIdp != IdentityProviders.BCServicesCard)
            {
                // If the party does not have a BC Services Card Credential, only linking to BCSC is allowed.
                this.logger.LogPartyDoesNotHaveBCServicesCard(command.PartyId);
                return DomainResult.Failed<CredentialLinkTicket>("Party does not have a BC Services Card Credential.");
            }

            if (idps.Contains(command.LinkToIdp))
            {
                this.logger.LogPartyHasExistingLinkedCredential(command.PartyId, command.LinkToIdp);
                return DomainResult.Failed<CredentialLinkTicket>($"Party already has a linked {command.LinkToIdp}.");
            }

            var ticket = new CredentialLinkTicket
            {
                Token = Guid.NewGuid(),
                PartyId = command.PartyId,
                Claimed = false,
                ExpiresAt = this.clock.GetCurrentInstant().Plus(CredentialLinkTicket.TicketLifetime),
                LinkToIdentityProvider = command.LinkToIdp
            };

            this.context.CredentialLinkTickets.Add(ticket);
            await this.context.SaveChangesAsync();

            return DomainResult.Success(ticket);
        }
    }
}

public static partial class CredentialLinkTicketCreateLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Error, "Party {partyId} initiated account linking but does not have a BC Services Card Credential.")]
    public static partial void LogPartyDoesNotHaveBCServicesCard(this ILogger<LinkTicketCreate.CommandHandler> logger, int partyId);

    [LoggerMessage(2, LogLevel.Error, "Party {partyId} attempted to link a second Credential of type {linkToIdp}.")]
    public static partial void LogPartyHasExistingLinkedCredential(this ILogger<LinkTicketCreate.CommandHandler> logger, int partyId, string linkToIdp);
}
