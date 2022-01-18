namespace Pidp.Features.Parties;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using HybridModelBinding;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using Pidp.Data;
using Pidp.Models.Lookups;

public class ProfileStatus
{
    public class Query : IQuery<Model>
    {
        public int Id { get; set; }
    }

    public class Model
    {
        [HybridBindProperty(Source.Route)]
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public LocalDate Birthdate { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public CollegeCode? CollegeCode { get; set; }
        public string? LicenceNumber { get; set; }
        public string? JobTitle { get; set; }
        public string? FacilityName { get; set; }
        public string? FacilityStreet { get; set; }

        public bool DemographicsComplete => this.Email != null && this.Phone != null;
        public bool CollegeCertificationComplete => this.Email != null && this.Phone != null;
        public bool WorkSettingComplete => this.Email != null && this.Phone != null;
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator() => this.RuleFor(x => x.Id).NotEmpty();
    }

    public class QueryHandler : IQueryHandler<Query, Model>
    {
        private readonly PidpDbContext context;
        private readonly IMapper mapper;

        public QueryHandler(PidpDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<Model> HandleAsync(Query query)
        {
            return await this.context.Parties
                .Where(party => party.Id == query.Id)
                .ProjectTo<Model>(this.mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }
    }
}
