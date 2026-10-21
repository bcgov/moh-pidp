namespace Pidp.Features.Pharmacies;

using Mediator;
using Microsoft.EntityFrameworkCore;
using DomainResults.Common;
using Pidp.Data;

public class EnrolmentTokenDetails
{
    public class Query : IRequest<IDomainResult<Model>>
    {
        public Guid Token { get; set; }
    }

    public class Model
    {
        public string PharmacyName { get; set; } = string.Empty;
    }

    public class QueryHandler(PidpDbContext context) : IRequestHandler<Query, IDomainResult<Model>>
    {
        public async ValueTask<IDomainResult<Model>> Handle(Query request, CancellationToken cancellationToken)
        {
            var details = await context.PharmacyEnrolments
                .Where(e => e.Token == request.Token)
                .Select(e => new Model
                {
                    PharmacyName = e.Pharmacy.Name
                })
                .SingleOrDefaultAsync(cancellationToken);

            if (details == null)
            {
                return DomainResult.NotFound<Model>();
            }

            return DomainResult.Success(details);
        }
    }
}
