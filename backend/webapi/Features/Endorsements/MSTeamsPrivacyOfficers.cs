namespace Pidp.Features.Endorsements;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using DomainResults.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

using Pidp.Data;
using Pidp.Models.Lookups;

public class MSTeamsPrivacyOfficers
{
    public class Query : IQuery<IDomainResult<List<Model>>>
    {
        public int PartyId { get; set; }
    }

    public class Model
    {
        public string FullName { get; set; } = string.Empty;
        public int ClinicId { get; set; }
        public string ClinicName { get; set; } = string.Empty;
        public Address ClinicAddress { get; set; } = new();

        public class Address
        {
            public string CountryCode { get; set; } = string.Empty;
            public string ProvinceCode { get; set; } = string.Empty;
            public string Street { get; set; } = string.Empty;
            public string City { get; set; } = string.Empty;
            public string Postal { get; set; } = string.Empty;
        }
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator() => this.RuleFor(x => x.PartyId).GreaterThan(0);
    }

    public class QueryHandler : IQueryHandler<Query, IDomainResult<List<Model>>>
    {
        private readonly IMapper mapper;
        private readonly PidpDbContext context;

        public QueryHandler(IMapper mapper, PidpDbContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public async Task<IDomainResult<List<Model>>> HandleAsync(Query query)
        {
            return DomainResult.Success(new List<Model>());
        }
    }
}
