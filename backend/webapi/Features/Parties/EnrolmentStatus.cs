namespace Pidp.Features.Parties;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using HybridModelBinding;
using Microsoft.EntityFrameworkCore;

using Pidp.Data;

public class EnrolmentStatus
{
    public class Query : IQuery<Model>
    {
        public int Id { get; set; }
    }

    public class Model
    {
        [HybridBindProperty(Source.Route)]
        public int Id { get; set; }
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator() => this.RuleFor(x => x.Id).NotEmpty();
    }

    public class QueryHandler : IQueryHandler<Query, Model>
    {
        private readonly PidpDbContext context;
        private readonly IMapper mapper;

        public QueryHandler(PidpDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<Model> HandleAsync(Query query)
        {
            return await this.context.Parties
                .Where(party => party.Id == query.Id)
                .ProjectTo<Model>(this.mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }
    }
}
