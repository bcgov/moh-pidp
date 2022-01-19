namespace PlrIntake.Features.Records;

using FluentValidation;
using Microsoft.EntityFrameworkCore;

using PlrIntake.Data;

public class Index
{
    public class Query : IQuery<List<Model>>
    {
        public string CollegeId { get; set; } = string.Empty;
    }

    public class Model
    {
        public string Ipc { get; set; } = string.Empty;
        public string? ProviderRoleType { get; set; }
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator() => this.RuleFor(x => x.CollegeId).NotEmpty();
    }

    public class QueryHandler : IQueryHandler<Query, List<Model>>
    {
        private readonly PlrDbContext context;

        public QueryHandler(PlrDbContext context) => this.context = context;

        public async Task<List<Model>> HandleAsync(Query query)
        {
            return await this.context.PlrRecords
                .Where(record => record.CollegeId == query.CollegeId)
                .Select(record => new Model
                {
                    Ipc = record.Ipc,
                    ProviderRoleType = record.ProviderRoleType
                })
                .ToListAsync();
        }
    }
}
