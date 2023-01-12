namespace PlrIntake.Features.Records;

using FluentValidation;
using Microsoft.EntityFrameworkCore;

using PlrIntake.Data;
using PlrIntake.Extensions;

public class Search
{
    public class Query : IQuery<List<string>>
    {
        public string CollegeId { get; set; } = string.Empty;
        public DateTime Birthdate { get; set; }
        public List<string> IdentifierTypes { get; set; } = new();
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator()
        {
            this.RuleFor(x => x.CollegeId).NotEmpty();
            this.RuleFor(x => x.Birthdate).NotEmpty();
            this.RuleFor(x => x.IdentifierTypes).NotEmpty();
            this.RuleForEach(x => x.IdentifierTypes).NotEmpty();
        }
    }

    public class QueryHandler : IQueryHandler<Query, List<string>>
    {
        private readonly PlrDbContext context;

        public QueryHandler(PlrDbContext context) => this.context = context;

        public async Task<List<string>> HandleAsync(Query query)
        {
            var paddedId = query.CollegeId
                .PadLeft(5, '0')
                [^5..];

            return await this.context.PlrRecords
                .ExcludeDeleted()
                .Where(record => record.CollegeId!.PadLeft(5, '0').EndsWith(paddedId)
                    && record.DateOfBirth!.Value.Date == query.Birthdate.Date
                    && query.IdentifierTypes.Contains(record.IdentifierType!))
                .Select(record => record.Cpn!) // All valid PLR records will have CPNs
                .ToListAsync();
        }
    }
}
