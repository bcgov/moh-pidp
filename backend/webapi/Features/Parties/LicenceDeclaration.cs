namespace Pidp.Features.Parties;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using DomainResults.Common;
using FluentValidation;
using HybridModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

using Pidp.Data;
using Pidp.Features;
using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models;
using Pidp.Models.Lookups;

public class LicenceDeclaration
{
    public class Query : IQuery<Command>
    {
        public int PartyId { get; set; }
    }

    public class Command : ICommand<IDomainResult<string?>>
    {
        [JsonIgnore]
        [HybridBindProperty(Source.Route)]
        public int PartyId { get; set; }
        public CollegeCode? CollegeCode { get; set; }
        public string? LicenceNumber { get; set; }
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator() => this.RuleFor(x => x.PartyId).GreaterThan(0);
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            this.RuleFor(x => x.PartyId).GreaterThan(0);
            this.When(x => x.CollegeCode.HasValue, () =>
            {
                this.RuleFor(x => x.CollegeCode).IsInEnum();
                this.RuleFor(x => x.LicenceNumber).NotEmpty();
            })
            .Otherwise(() => this.RuleFor(x => x.LicenceNumber).Null());
        }
    }

    public class QueryHandler : IQueryHandler<Query, Command>
    {
        private readonly IMapper mapper;
        private readonly PidpDbContext context;

        public QueryHandler(IMapper mapper, PidpDbContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public async Task<Command> HandleAsync(Query query)
        {
            var cert = await this.context.PartyLicenceDeclarations
                .Where(licence => licence.PartyId == query.PartyId)
                .ProjectTo<Command>(this.mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            return cert ?? new Command { PartyId = query.PartyId };
        }
    }

    public class CommandHandler : ICommandHandler<Command, IDomainResult<string?>>
    {
        private readonly IKeycloakAdministrationClient keycloakClient;
        private readonly ILogger logger;
        private readonly IPlrClient plrClient;
        private readonly PidpDbContext context;

        public CommandHandler(
            IKeycloakAdministrationClient keycloakClient,
            ILogger<CommandHandler> logger,
            IPlrClient plrClient,
            PidpDbContext context)
        {
            this.keycloakClient = keycloakClient;
            this.logger = logger;
            this.plrClient = plrClient;
            this.context = context;
        }

        public async Task<IDomainResult<string?>> HandleAsync(Command command)
        {
            var party = await this.context.Parties
                .Include(party => party.LicenceDeclaration)
                .SingleAsync(party => party.Id == command.PartyId);

            if (!string.IsNullOrWhiteSpace(party.Cpn))
            {
                // Users cannot update licence declarations once found in PLR
                return DomainResult.Failed<string?>();
            }

            party.LicenceDeclaration ??= new PartyLicenceDeclaration();
            party.LicenceDeclaration.CollegeCode = command.CollegeCode;
            party.LicenceDeclaration.LicenceNumber = command.LicenceNumber;

            party.Cpn = command.CollegeCode == null || command.LicenceNumber == null || party.Birthdate == null
                ? null // Declared "No Licence"
                : await this.plrClient.FindCpnAsync(command.CollegeCode.Value, command.LicenceNumber, party.Birthdate.Value);

            await this.context.SaveChangesAsync();

            if (party.Cpn != null)
            {
                if (!await this.keycloakClient.UpdateUser(party.UserId, (user) => user.SetCpn(party.Cpn)))
                {
                    this.logger.LogCpnAssignmentFailure(party.UserId);
                }
            }

            return DomainResult.Success(party.Cpn);
        }
    }
}

public static partial class LicenceDeclarationLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Error, "Could not assign a CPN to user {userId}.")]
    public static partial void LogCpnAssignmentFailure(this ILogger logger, Guid userId);
}
