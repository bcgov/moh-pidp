namespace Pidp.Features.Pharmacies;

using Pidp.Models.Lookups;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using Pidp.Data;
using Pidp.Extensions;

public class PharmacyIndex
{
    public class Query : IQuery<List<Model>>
    {
        public int PartyId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string HealthAuthority { get; set; } = string.Empty;
        public string Address1 { get; set; } = string.Empty;
        public string Address2 { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Province { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string ManagerName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Fax { get; set; } = string.Empty;
        public string PharmaCareCode { get; set; } = string.Empty;
        public bool IsCareConnectCompleted { get; set; }
        public DateTime? VerifiedCareConnectCompletedDate { get; set; }
        public string? VerifiedCareConnectCompleted { get; set; } = string.Empty;
    }

    public class Model
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string HealthAuthority { get; set; } = string.Empty;
        public string Address1 { get; set; } = string.Empty;
        public string? Address2 { get; set; }
        public string City { get; set; } = string.Empty;
        public string Province { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string ManagerName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Fax { get; set; } = string.Empty;
        public string PharmaCareCode { get; set; } = string.Empty;
        public bool IsCareConnectCompleted { get; set; }
        public DateTime? VerifiedCareConnectCompletedDate { get; set; }
        public string? VerifiedCareConnectCompleted { get; set; } = string.Empty;
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator() => this.RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
    }

    public class QueryHandler(PidpDbContext context) : IQueryHandler<Query, List<Model>>
    {
        private readonly PidpDbContext context = context;

        public async Task<List<Model>> HandleAsync(Query query)
        {
            return await this.context.Pharmacies.Include(pharmacy => pharmacy.Staff).Select(pharmacy => new Model
            {
                Id = pharmacy.Id,
                Name = pharmacy.Name,
                HealthAuthority = pharmacy.HealthAuthority,
                Address1 = pharmacy.Address1,
                Address2 = pharmacy.Address2,
                City = pharmacy.City,
                Province = pharmacy.Province,
                PostalCode = pharmacy.PostalCode,
                ManagerName = pharmacy.ManagerName,
                Email = pharmacy.Email,
                Phone = pharmacy.Phone,
                Fax = pharmacy.Fax,
                PharmaCareCode = pharmacy.PharmaCareCode,
                IsCareConnectCompleted = pharmacy.IsCareConnectCompleted,
                VerifiedCareConnectCompletedDate = pharmacy.VerifiedCareConnectCompletedDate,
                VerifiedCareConnectCompleted = pharmacy.VerifiedCareConnectCompleted
            }).ToListAsync();
        }
    }
}
