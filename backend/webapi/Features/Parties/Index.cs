namespace Pidp.Features.Parties;

using FluentValidation;
using Microsoft.EntityFrameworkCore;

using Pidp.Data;
using Pidp.Extensions;

public class Index
{
    public class Query : IQuery<List<Model>>
    {
        public Guid UserId { get; set; }
    }

    public class Model
    {
        public int Id { get; set; }
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator(IHttpContextAccessor accessor)
        {
            var user = accessor?.HttpContext?.User;
            this.RuleFor(x => x.UserId).NotEmpty().Equal(user.GetUserId());
        }
    }

    public class QueryHandler : IQueryHandler<Query, List<Model>>
    {
        private readonly PidpDbContext context;

        public QueryHandler(PidpDbContext context) => this.context = context;

        public async Task<List<Model>> HandleAsync(Query query)
        {
            return await this.context.Parties
                .Where(party => party.UserId == query.UserId)
                .Select(party => new Model
                {
                    Id = party.Id
                })
                .ToListAsync();
        }
    }
}
