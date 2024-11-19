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

public class PHR
{
    public static IdentifierType[] ExcludedIdentifierTypes => [IdentifierType.PharmacyTech];

    public static bool IsEligible(PlrStandingsDigest partyPlrStanding)
    {
        return partyPlrStanding
            .Excluding(ExcludedIdentifierTypes)
            .HasGoodStanding || partyPlrStanding.IsCpsPostgrad;
    }

    public class Command : ICommand<IDomainResult>
    {
        public int PartyId { get; set; }
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
                    AlreadyEnroled = party.AccessRequests.Any(request => request.AccessTypeCode == AccessTypeCode.PHR),
                    UserIds = party.Credentials
                        .Where(credential => credential.IdentityProvider == IdentityProviders.BCServicesCard || credential.IdentityProvider == IdentityProviders.BCProvider)
                        .Select(credential => credential.UserId),
                    party.Email,
                    party.DisplayFirstName,
                    party.Cpn,
                })
                .SingleAsync();

            if (dto.AlreadyEnroled
                || dto.Email == null
                || !IsEligible(await this.plrClient.GetStandingsDigestAsync(dto.Cpn)))
            {
                this.logger.LogAccessRequestDenied();
                return DomainResult.Failed();
            }

            foreach (var userId in dto.UserIds)
            {
                if (!await this.keycloakClient.AssignAccessRoles(userId, MohKeycloakEnrolment.PHR))
                {
                    return DomainResult.Failed();
                }
            }

            this.context.AccessRequests.Add(new AccessRequest
            {
                PartyId = command.PartyId,
                AccessTypeCode = AccessTypeCode.PHR,
                RequestedOn = this.clock.GetCurrentInstant()
            });

            await this.context.SaveChangesAsync();

            return DomainResult.Success();
        }

    }
}

public static partial class PHRLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Warning, "PHR Access Request denied due to the Party Record not meeting all prerequisites.")]
    public static partial void LogAccessRequestDenied(this ILogger<PHR.CommandHandler> logger);
}
