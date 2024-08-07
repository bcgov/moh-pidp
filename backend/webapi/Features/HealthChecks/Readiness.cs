namespace Pidp.Features.HealthChecks;

using DomainResults.Common;

using Pidp.Data;

public class Readiness
{
    public class Query : IQuery<IDomainResult> { }

    public class QueryHandler(PidpDbContext context) : IQueryHandler<Query, IDomainResult>
    {
        private readonly PidpDbContext context = context;

        public async Task<IDomainResult> HandleAsync(Query query)
        {
            return await this.context.Database.CanConnectAsync()
                ? DomainResult.Success()
                : DomainResult.Failed();
        }
    }
}
