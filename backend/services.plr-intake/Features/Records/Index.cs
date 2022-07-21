namespace PlrIntake.Features.Records;

using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

using PlrIntake.Data;
using PlrIntake.Extensions;

public class Index
{
    public class Query : IQuery<List<Model>>
    {
        public string? Cpn { get; set; }
        public string CollegeId { get; set; } = string.Empty;
        public DateTime Birthdate { get; set; }
        public List<string> IdentifierTypes { get; set; } = new();

        [JsonIgnore]
        public bool FilterByCpn => this.Cpn != null;
    }

    public class Model
    {
        public string Cpn { get; set; } = string.Empty;
        public string Ipc { get; set; } = string.Empty;
        public string? IdentifierType { get; set; }
        public string? CollegeId { get; set; }
        public string? ProviderRoleType { get; set; }
        public string? StatusCode { get; set; }
        public DateTime? StatusStartDate { get; set; }
        public string? StatusReasonCode { get; set; }
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator()
        {
            this.When(query => !query.FilterByCpn, () =>
            {
                this.RuleFor(x => x.CollegeId).NotEmpty();
                this.RuleFor(x => x.Birthdate).NotEmpty();
                this.RuleFor(x => x.IdentifierTypes).NotEmpty();
                this.RuleForEach(x => x.IdentifierTypes).NotEmpty();
            });
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
                .If(query.FilterByCpn, q => q
                    .Where(record => record.Cpn == query.Cpn))
                .If(!query.FilterByCpn, q => q
                    .Where(record => record.CollegeId!.TrimStart('0') == query.CollegeId.TrimStart('0')
                        && record.DateOfBirth!.Value.Date == query.Birthdate.Date
                        && query.IdentifierTypes.Contains(record.IdentifierType!)))
                .Select(record => new Model
                {
                    Cpn = record.Cpn!, // All valid PLR records will have CPNs
                    Ipc = record.Ipc,
                    IdentifierType = record.IdentifierType,
                    CollegeId = record.CollegeId,
                    ProviderRoleType = record.ProviderRoleType,
                    StatusCode = record.StatusCode,
                    StatusStartDate = record.StatusStartDate,
                    StatusReasonCode = record.StatusReasonCode
                })
                .ToListAsync();
        }
    }
}
