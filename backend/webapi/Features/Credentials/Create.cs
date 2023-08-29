namespace Pidp.Features.Credentials;

using DomainResults.Common;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using System.Text.Json.Serialization;
using System.Security.Claims;

using Pidp.Data;
using Pidp.Extensions;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.BCProvider;
using Pidp.Models;
using Pidp.Models.Lookups;
using Pidp.Models.DomainEvents;

public class Create
{
    public class Command : ICommand<IDomainResult<int>>
    {
        [JsonIgnore]
        public Guid CredentialLinkToken { get; set; }
        [JsonIgnore]
        public ClaimsPrincipal User { get; set; } = new();
    }

    public class CommandHandler : ICommandHandler<Command, IDomainResult<int>>
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

        public async Task<IDomainResult<int>> HandleAsync(Command command)
        {
            var userId = command.User.GetUserId();
            var userIdentityProvider = command.User.GetIdentityProvider();
            var userIdpId = command.User.GetIdpId();
            if (userId == Guid.Empty
                || string.IsNullOrWhiteSpace(userIdentityProvider)
                || string.IsNullOrWhiteSpace(userIdpId))
            {
                this.logger.LogCredentialLinkTicketUserError(userId, userIdentityProvider, userIdpId);
                return DomainResult.Failed<int>();
            }

            var ticket = await this.context.CredentialLinkTickets
                .Where(ticket => ticket.Token == command.CredentialLinkToken
                    && !ticket.Claimed)
                .SingleOrDefaultAsync();

            if (ticket == null)
            {
                this.logger.LogCredentialLinkTicketNotFound(command.CredentialLinkToken);
                return DomainResult.NotFound<int>();
            }
            if (ticket.ExpiresAt < this.clock.GetCurrentInstant())
            {
                this.logger.LogCredentialLinkTicketExpired(ticket.Id);
                return DomainResult.Failed<int>();
            }
            if (ticket.LinkToIdentityProvider != userIdentityProvider)
            {
                this.logger.LogCredentialLinkTicketIdpError(ticket.Id, ticket.LinkToIdentityProvider, userIdentityProvider);
                return DomainResult.Failed<int>();
            }

            var credential = new Credential
            {
                PartyId = ticket.PartyId,
                UserId = userId,
                IdentityProvider = userIdentityProvider,
                IdpId = userIdpId
            };
            credential.DomainEvents.Add(new CredentialLinked(credential));
            this.context.Credentials.Add(credential);

            ticket.Claimed = true;

            await this.context.SaveChangesAsync();

            return DomainResult.Success(ticket.PartyId);
        }
    }


    public class CredentailLinkedHandler : INotificationHandler<CredentialLinked>
    {
        private readonly IBCProviderClient bcProviderClient;
        private readonly PidpDbContext context;
        private readonly string bcProviderClientId;

        public CredentailLinkedHandler(
            IBCProviderClient bcProviderClient,
            PidpConfiguration config,
            PidpDbContext context)
        {
            this.bcProviderClient = bcProviderClient;
            this.context = context;
            this.bcProviderClientId = config.BCProviderClient.ClientId;
        }

        public async Task Handle(CredentialLinked notification, CancellationToken cancellationToken)
        {
            // TODO: update AAD
            // var bcProviderId = await this.context.Credentials
            //     .Where(credential => credential.PartyId == notification.PartyId
            //         && credential.IdentityProvider == IdentityProviders.BCProvider)
            //     .Select(credential => credential.IdpId)
            //     .SingleOrDefaultAsync(cancellationToken);

            // if (string.IsNullOrEmpty(bcProviderId))
            // {
            //     return;
            // }


        }
    }
}

public static partial class CredentialCreateLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Error, "User object is missing one or more required properties: User ID = {userId}, IDP = {idp}, IDP ID = {idpId}.")]
    public static partial void LogCredentialLinkTicketUserError(this ILogger logger, Guid userId, string? idp, string? idpId);

    [LoggerMessage(2, LogLevel.Error, "A unclaimed Credential Link Ticket with token {credentialLinkToken} was not found.")]
    public static partial void LogCredentialLinkTicketNotFound(this ILogger logger, Guid credentialLinkToken);

    [LoggerMessage(3, LogLevel.Error, "Credential Link Ticket {credentialLinkTicketId} is expired.")]
    public static partial void LogCredentialLinkTicketExpired(this ILogger logger, int credentialLinkTicketId);

    [LoggerMessage(4, LogLevel.Error, "Credential Link Ticket {credentialLinkTicketId} expected to link to IDP {expectedIdp}, user had IDP {actualIdp} instead.")]
    public static partial void LogCredentialLinkTicketIdpError(this ILogger logger, int credentialLinkTicketId, string expectedIdp, string? actualIdp);
}
