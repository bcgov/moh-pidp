namespace Pidp.Features.Parties;

using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NodaTime;

using Pidp.Data;
using Pidp.Features;
using Pidp.Infrastructure.HttpClients.Plr;

public class CollegeCertificationIndex
{
    public class Query : IQuery<List<Model>>
    {
        public int PartyId { get; set; }
    }

    public class Model
    {
        public string? IdentifierType { get; set; }
        public string? ProviderRoleType { get; set; }
        public string? StatusCode { get; set; }
        public LocalDate? StatusStartDate { get; set; }
        public string? StatusReasonCode { get; set; }

        public bool IsGoodStanding { get; set; }
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator() => this.RuleFor(x => x.PartyId).GreaterThan(0);
    }

    public class QueryHandler : IQueryHandler<Query, List<Model>>
    {
        private readonly IMapper mapper;
        private readonly IPlrClient client;
        private readonly PidpDbContext context;

        public QueryHandler(
            IMapper mapper,
            IPlrClient client,
            PidpDbContext context)
        {
            this.mapper = mapper;
            this.client = client;
            this.context = context;
        }

        public async Task<List<Model>> HandleAsync(Query query)
        {
            var cpn = await this.context.Parties
                .Where(party => party.Id == query.PartyId)
                .Select(party => party.Cpn)
                .SingleAsync();

            if (string.IsNullOrWhiteSpace(cpn))
            {
                return new();
            }

            var records = await this.client.GetRecords(cpn);
            if (records == null)
            {
                // TODO  what to do on error?
                return new();
            }

            return this.mapper.Map<List<Model>>(records);
        }
    }
}
