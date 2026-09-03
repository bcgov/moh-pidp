namespace Pidp.Features.Pharmacies;

using FluentValidation;
using Mediator;
using Microsoft.EntityFrameworkCore;
using DomainResults.Common;

using Pidp.Data;

public class PharmacySearch
{
    public class Query : IRequest<IDomainResult<List<Model>>>
    {
        public string QueryString { get; set; } = string.Empty;
    }

    public class Model
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Fax { get; set; } = string.Empty;
        public string PharmaCareCode { get; set; } = string.Empty;
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator() => this.RuleFor(x => x.QueryString).MinimumLength(3).WithMessage("Search query must be at least 3 characters.");
    }

    public class QueryHandler(PidpDbContext context) : IRequestHandler<Query, IDomainResult<List<Model>>>
    {
        private readonly PidpDbContext context = context;

        public async ValueTask<IDomainResult<List<Model>>> Handle(Query query, CancellationToken cancellationToken)
        {
            var searchString = query.QueryString.ToLower();
            
            var matches = await this.context.Pharmacies
                .Where(pharmacy => pharmacy.ManagerId == null && (pharmacy.Name.ToLower().Contains(searchString) || pharmacy.PharmaCareCode.ToLower().Contains(searchString)))
                .Take(51) // Take one extra to see if we exceed 50
                .Select(pharmacy => new Model
                {
                    Id = pharmacy.Id,
                    Name = pharmacy.Name,
                    Address = pharmacy.Address,
                    Email = pharmacy.Email,
                    Phone = pharmacy.Phone,
                    Fax = pharmacy.Fax,
                    PharmaCareCode = pharmacy.PharmaCareCode
                })
                .ToListAsync();

            if (matches.Count > 50)
            {
                return DomainResult.Failed<List<Model>>("too many to list");
            }

            return DomainResult.Success(matches);
        }
    }
}
