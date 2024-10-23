namespace Pidp.Features.Parties;

using AutoMapper;
using DomainResults.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NodaTime;

using Pidp.Data;
using Pidp.Features;
using Pidp.Infrastructure.HttpClients.Plr;

public class CollegeCertifications
{
    public class Query : IQuery<IDomainResult<List<Model>>>
    {
        public int PartyId { get; set; }
    }

    public class Model
    {
        public string? IdentifierType { get; set; }
        public string? CollegeId { get; set; }
        public string? ProviderRoleType { get; set; }
        public string? StatusCode { get; set; }
        public LocalDate? StatusStartDate { get; set; }

        public bool IsGoodStanding { get; set; }
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator() => this.RuleFor(x => x.PartyId).GreaterThan(0);
    }

    public class QueryHandler(
        ILogger<QueryHandler> logger,
        IMapper mapper,
        IPlrClient client,
        PidpDbContext context) : IQueryHandler<Query, IDomainResult<List<Model>>>
    {
        private readonly ILogger<QueryHandler> logger = logger;
        private readonly IMapper mapper = mapper;
        private readonly IPlrClient client = client;
        private readonly PidpDbContext context = context;

        public async Task<IDomainResult<List<Model>>> HandleAsync(Query query)
        {
            var cpn = await this.context.Parties
                .Where(party => party.Id == query.PartyId)
                .Select(party => party.Cpn)
                .SingleAsync();

            if (string.IsNullOrWhiteSpace(cpn))
            {
                return DomainResult.Success(new List<Model>());
            }

            var records = await this.client.GetRecordsAsync(cpn);
            if (records == null)
            {
                return DomainResult.Failed<List<Model>>();
            }
            if (!records.Any())
            {
                this.logger.LogNoCertsFound(cpn);
                return DomainResult.Failed<List<Model>>();
            }

            return DomainResult.Success(this.mapper.Map<List<Model>>(records));
        }
    }
}

public static partial class CollegeCertificationLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Warning, "Unexpected result of no Records in PLR for user with CPN = {cpn}.")]
    public static partial void LogNoCertsFound(this ILogger<CollegeCertifications.QueryHandler> logger, string cpn);
}
