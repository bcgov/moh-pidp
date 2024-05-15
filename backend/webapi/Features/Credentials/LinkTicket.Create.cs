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
            this.RuleFor(x => x.PartyId).GreaterThan(0);
            this.RuleFor(x => x.LinkToIdp).NotEmpty()
                .Equal(IdentityProviders.BCProvider).WithMessage("Currently, only linking from BC Services Card to BC Provider is supported.");
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

            if (!idps.Contains(IdentityProviders.BCServicesCard))
            {
                this.logger.LogPartyDoesNotHaveBCServicesCard(command.PartyId);
                return DomainResult.Failed<CredentialLinkTicket>("Party does not have a BC Services Card Credential.");
            }
            if (idps.Contains(IdentityProviders.BCProvider))
            {
                this.logger.LogPartyHasExistingLinkedBCProviderCredential(command.PartyId);
                return DomainResult.Failed<CredentialLinkTicket>("Party already has a linked BC Provider Credential.");
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
    [LoggerMessage(1, LogLevel.Error, "Party {partyId} initiated account linking but does not have a BC Services Card Creential.")]
    public static partial void LogPartyDoesNotHaveBCServicesCard(this ILogger<LinkTicketCreate.CommandHandler> logger, int partyId);

    [LoggerMessage(2, LogLevel.Error, "Party {partyId} attempted to link a second BC Provider account.")]
    public static partial void LogPartyHasExistingLinkedBCProviderCredential(this ILogger<LinkTicketCreate.CommandHandler> logger, int partyId);
}
