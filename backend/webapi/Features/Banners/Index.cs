namespace Pidp.Features.Banners;

using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NodaTime;

using Pidp.Data;
using Pidp.Models;

public class Index
{
    public class Query : IQuery<List<Model>>
    {
        public string Component { get; set; } = string.Empty;
    }

    public class Model
    {
        public string Component { get; set; } = string.Empty;
        public BannerStatus Status { get; set; }
        public string Header { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator() => this.RuleFor(x => x.Component).NotEmpty();
    }

    public class QueryHandler(IClock clock, PidpDbContext context) : IQueryHandler<Query, List<Model>>
    {
        private readonly IClock clock = clock;
        private readonly PidpDbContext context = context;

        public async Task<List<Model>> HandleAsync(Query query)
        {
            return await this.context.Banners
                .Where(banner => banner.Component == query.Component
                    && banner.StartTime < this.clock.GetCurrentInstant()
                    && banner.EndTime > this.clock.GetCurrentInstant())
                .Select(banner => new Model
                {
                    Component = banner.Component,
                    Status = banner.Status,
                    Header = banner.Header,
                    Body = banner.Body,
                })
                .ToListAsync();
        }
    }
}
