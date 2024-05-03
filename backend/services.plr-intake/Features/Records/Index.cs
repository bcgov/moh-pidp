namespace PlrIntake.Features.Records;

using FluentValidation;
using Microsoft.EntityFrameworkCore;

using PlrIntake.Data;
using PlrIntake.Extensions;

public class Index
{
    public class Query : IQuery<List<Model>>
    {
        public List<string> Cpns { get; set; } = new();
    }

    public class Model
    {
        public string Cpn { get; set; } = string.Empty;
        public string? IdentifierType { get; set; }
        public string? CollegeId { get; set; }
        public string? ProviderRoleType { get; set; }
        public string? StatusCode { get; set; }
        public DateTime? StatusStartDate { get; set; }
        public string? StatusReasonCode { get; set; }
        public string? MspId { get; set; }
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator()
        {
            this.RuleFor(x => x.Cpns).NotEmpty();
            this.RuleForEach(x => x.Cpns).NotEmpty();
        }
    }

    public class QueryHandler : IQueryHandler<Query, List<Model>>
    {
        private readonly PlrDbContext context;

        public QueryHandler(PlrDbContext context) => this.context = context;

        public async Task<List<Model>> HandleAsync(Query query)
        {
            return await this.context.PlrRecords
                .ExcludeDeleted()
                .Where(record => record.Cpn != null
                    && query.Cpns.Contains(record.Cpn))
                .Select(record => new Model
                {
                    Cpn = record.Cpn!,
                    IdentifierType = record.IdentifierType,
                    CollegeId = record.CollegeId,
                    ProviderRoleType = record.ProviderRoleType,
                    StatusCode = record.StatusCode,
                    StatusStartDate = record.StatusStartDate,
                    StatusReasonCode = record.StatusReasonCode,
                    MspId = record.MspId
                })
                .ToListAsync();
        }
    }
}
