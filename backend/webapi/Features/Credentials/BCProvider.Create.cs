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
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Extensions;

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
        private readonly IPlrClient plrClient;

        public CommandHandler(
            IBCProviderClient client,
            PidpDbContext context,
            ILogger<CommandHandler> logger,
            IPlrClient plrClient)
        {
            this.client = client;
            this.context = context;
            this.logger = logger;
            this.plrClient = plrClient;
        }

        public async Task<IDomainResult> HandleAsync(Command command)
        {
            var party = await this.context.Parties
                .Where(party => party.Id == command.PartyId)
                .Select(party => new
                {
                    party.FirstName,
                    party.LastName,
                    party.Cpn,
                    party.Email,
                    HasBCProviderCredential = party.Credentials.Any(credential => credential.IdentityProvider == IdentityProviders.BCProvider),
                    Hpdid = party.Credentials.Select(credential => credential.Hpdid).Single(hpdid => hpdid != null),
                })
                .SingleAsync();

            if (party.HasBCProviderCredential)
            {
                this.logger.LogPartyHasBCProviderCredential(command.PartyId);
                return DomainResult.Failed();
            }

            if (party.Email == null
                || party.Hpdid == null)
            {
                this.logger.LogInvalidState(command.PartyId, party);
                return DomainResult.Failed();
            }

            var partyPlrStanding = await this.plrClient.GetStandingsDigestAsync(party.Cpn);

            var newUserRep = new NewUserRepresentation
            {
                FirstName = party.FirstName,
                LastName = party.LastName,
                Hpdid = party.Hpdid,
                IsRnp = partyPlrStanding.With(ProviderRoleType.RegisteredNursePractitioner).HasGoodStanding,
                IsMd = partyPlrStanding.With(ProviderRoleType.MedicalDoctor).HasGoodStanding,
                Cpn = party.Cpn,
                Password = command.Password,
                PidpEmail = party.Email
            };

            if (party.Cpn == null)
            {
                var endorsementCpns = await this.context.ActiveEndorsementRelationships(command.PartyId)
                    .Select(relationship => relationship.Party!.Cpn)
                    .ToListAsync();
                var endorsementPlrStanding = await this.plrClient.GetAggregateStandingsDigestAsync(endorsementCpns);

                newUserRep.IsMoa = endorsementPlrStanding.HasGoodStanding;
            }

            var createdUser = await this.client.CreateBCProviderAccount(newUserRep);

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

    [LoggerMessage(2, LogLevel.Error, "Failed to create BC Provider for Party {partyId}, one or more requirements were not met. Party state: {state}.")]
    public static partial void LogInvalidState(this ILogger logger, int partyId, object state);
}
