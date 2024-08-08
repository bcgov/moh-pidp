namespace Pidp.Features.AccessRequests;

using DomainResults.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NodaTime;

using Pidp.Data;
using Pidp.Extensions;
using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models;
using Pidp.Models.Lookups;

public class DriverFitness
{
    public static IdentifierType[] AllowedIdentifierTypes => [IdentifierType.PhysiciansAndSurgeons];

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
                    AlreadyEnroled = party.AccessRequests.Any(request => request.AccessTypeCode == AccessTypeCode.DriverFitness),
                    LicenceDeclarationCompleted = party.LicenceDeclaration != null,
                    UserId = party.PrimaryUserId,
                    party.Cpn,
                    party.Email
                })
                .SingleAsync();

            if (dto.AlreadyEnroled
                || dto.Email == null
                || !dto.LicenceDeclarationCompleted)
            {
                this.logger.LogAccessRequestDenied();
                return DomainResult.Failed();
            }

            if (dto.Cpn == null)
            {
                // Check status of Endorsements
                var endorsementCpns = await this.context.ActiveEndorsementRelationships(command.PartyId)
                    .Select(relationship => relationship.Party!.Cpn)
                    .ToListAsync();

                var endorsementPlrStanding = await this.plrClient.GetAggregateStandingsDigestAsync(endorsementCpns);
                if (!endorsementPlrStanding.HasGoodStanding)
                {
                    this.logger.LogAccessRequestDenied();
                    return DomainResult.Failed();
                }
            }
            else
            {
                // Check status of College Licence
                if (!(await this.plrClient.GetStandingsDigestAsync(dto.Cpn))
                    .With(AllowedIdentifierTypes)
                    .HasGoodStanding)
                {
                    this.logger.LogAccessRequestDenied();
                    return DomainResult.Failed();
                }
            }

            if (!await this.keycloakClient.AssignAccessRoles(dto.UserId, MohKeycloakEnrolment.DriverFitness))
            {
                return DomainResult.Failed();
            }

            this.context.AccessRequests.Add(new AccessRequest
            {
                PartyId = command.PartyId,
                AccessTypeCode = AccessTypeCode.DriverFitness,
                RequestedOn = this.clock.GetCurrentInstant()
            });

            await this.context.SaveChangesAsync();

            return DomainResult.Success();
        }
    }
}

public static partial class DriverFitnessLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Warning, "Driver Fitness Access Request denied due to the Party Record not meeting all prerequisites.")]
    public static partial void LogAccessRequestDenied(this ILogger<DriverFitness.CommandHandler> logger);
}
