namespace Pidp.Features.Parties;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using DomainResults.Common;
using FluentValidation;
using HybridModelBinding;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using System.Text.Json.Serialization;

using Pidp.Data;
using Pidp.Features;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models;
using Pidp.Models.DomainEvents;
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

    public class QueryHandler(IMapper mapper, PidpDbContext context) : IQueryHandler<Query, Command>
    {
        private readonly IMapper mapper = mapper;
        private readonly PidpDbContext context = context;

        public async Task<Command> HandleAsync(Query query)
        {
            var cert = await this.context.PartyLicenceDeclarations
                .Where(licence => licence.PartyId == query.PartyId)
                .ProjectTo<Command>(this.mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            return cert ?? new Command { PartyId = query.PartyId };
        }
    }

    public class CommandHandler(IPlrClient plrClient, PidpDbContext context, IClock clock, ILogger<LicenceDeclaration> logger) : ICommandHandler<Command, IDomainResult<string?>>
    {
        private readonly IPlrClient plrClient = plrClient;
        private readonly PidpDbContext context = context;
        private readonly IClock clock = clock;
        private readonly ILogger<LicenceDeclaration> logger = logger;

        public async Task<IDomainResult<string?>> HandleAsync(Command command)
        {
            var party = await this.context.Parties
                .Include(party => party.Credentials)
                .Include(party => party.LicenceDeclaration)
                .SingleAsync(party => party.Id == command.PartyId);

            if (!string.IsNullOrWhiteSpace(party.Cpn))
            {
                // Users cannot update licence declarations once found in PLR
                return DomainResult.Failed<string?>();
            }

            party.LicenceDeclaration ??= new();
            party.LicenceDeclaration.CollegeCode = command.CollegeCode;
            party.LicenceDeclaration.LicenceNumber = command.LicenceNumber;

            try
            {
                await party.HandleLicenceSearch(this.plrClient, this.context);
            }
            catch (Exception ex)
            {
                this.context.BusinessEvents.Add(CollegeLicenceSearchError.Create(party.Id, command.CollegeCode, this.clock.GetCurrentInstant()));
                this.logger.LogCollegeLicenceSearchError(ex.Message);
            }

            if (command.CollegeCode != null && string.IsNullOrWhiteSpace(party.Cpn))
            {
                party.DomainEvents.Add(new PlrCpnLookupNotFound(party.Id, command.CollegeCode.Value, command.LicenceNumber!));
            }

            await this.context.SaveChangesAsync();

            return DomainResult.Success(party.Cpn);
        }
    }

    public class PlrCpnLookupNotFoundHandler(IClock clock, PidpDbContext context) : INotificationHandler<PlrCpnLookupNotFound>
    {
        private readonly IClock clock = clock;
        private readonly PidpDbContext context = context;

        public Task Handle(PlrCpnLookupNotFound notification, CancellationToken cancellationToken)
        {
            this.context.BusinessEvents.Add(PartyNotInPlr.Create(notification.PartyId, notification.CollegeCode, notification.LicenceNumber, this.clock.GetCurrentInstant()));

            return Task.CompletedTask;
        }
    }
}

public static partial class LicenceDeclarationLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Error, "{message}.")]
    public static partial void LogCollegeLicenceSearchError(this ILogger logger, string message);
}
