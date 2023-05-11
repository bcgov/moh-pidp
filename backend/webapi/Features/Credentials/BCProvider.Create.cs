namespace Pidp.Features.Credentials;

using DomainResults.Common;
using FluentValidation;
using HybridModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

using Pidp.Data;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.BCProvider;
using Pidp.Models;

public class BCProviderCreate
{
    public class Command : ICommand<IDomainResult>
    {
        [JsonIgnore]
        [HybridBindProperty(Source.Route)]
        public int PartyId { get; set; }
        public string Password { get; set; } = string.Empty;
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            this.RuleFor(x => x.PartyId).GreaterThan(0);
            this.RuleFor(x => x.Password).NotEmpty();
        }
    }

    public class CommandHandler : ICommandHandler<Command, IDomainResult>
    {
        private readonly IBCProviderClient client;
        private readonly PidpDbContext context;
        private readonly ILogger logger;

        public CommandHandler(
            IBCProviderClient client,
            PidpDbContext context,
            ILogger<CommandHandler> logger)
        {
            this.client = client;
            this.context = context;
            this.logger = logger;
        }

        public async Task<IDomainResult> HandleAsync(Command command)
        {
            var party = await this.context.Parties
                .Where(party => party.Id == command.PartyId)
                .Select(party => new
                {
                    party.FirstName,
                    party.LastName,
                    HasBCProviderCredential = party.Credentials.Any(credential => credential.IdentityProvider == IdentityProviders.BCProvider),
                    Hpdid = party.Credentials.Select(credential => credential.Hpdid).Single(hpdid => hpdid != null),
                    party.Email
                })
                .SingleAsync();

            if (party.HasBCProviderCredential)
            {
                this.logger.LogPartyHasBCProviderCredential(command.PartyId);
                return DomainResult.Failed();
            }

            if (party.Hpdid == null)
            {
                this.logger.LogNullHpdid(command.PartyId);
                return DomainResult.Failed();
            }

            if (party.Email == null)
            {
                this.logger.LogNullEmail(command.PartyId);
            }

            var createdUser = await this.client.CreateBCProviderAccount(new UserRepresentation
            {
                FirstName = party.FirstName,
                LastName = party.LastName,
                Password = command.Password,
                Hpdid = party.Hpdid,
                PidpEmail = party.Email
            });

            if (createdUser == null)
            {
                return DomainResult.Failed();
            }

            this.context.Credentials.Add(new Credential
            {
                PartyId = command.PartyId,
                IdpId = createdUser.UserPrincipalName,
                IdentityProvider = IdentityProviders.BCProvider
            });

            await this.context.SaveChangesAsync();

            return DomainResult.Success();
        }
    }
}

public static partial class BCProviderCreateLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Information, "Party {partyId} attempted to create a second BC Provider account.")]
    public static partial void LogPartyHasBCProviderCredential(this ILogger logger, int partyId);

    [LoggerMessage(2, LogLevel.Error, "Failed to create BC Provider for Party {partyId}, HPDID was null.")]
    public static partial void LogNullHpdid(this ILogger logger, int partyId);
}
    [LoggerMessage(3, LogLevel.Information, "Email for Party {partyId} was null.")]
    public static partial void LogNullEmail(this ILogger logger, int partyId);
}
