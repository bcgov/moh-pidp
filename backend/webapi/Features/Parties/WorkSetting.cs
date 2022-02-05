namespace Pidp.Features.Parties;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using DomainResults.Common;
using FluentValidation;
using HybridModelBinding;
using Microsoft.EntityFrameworkCore;

using Pidp.Data;
using Pidp.Features;
using Pidp.Models;

public class WorkSetting
{
    public class Query : IQuery<IDomainResult<Command>>
    {
        public int Id { get; set; }
    }

    public class Command : ICommand<IDomainResult>
    {
        [HybridBindProperty(Source.Route)]
        public int Id { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string FacilityName { get; set; } = string.Empty;

        public Address? PhysicalAddress { get; set; }

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
        public QueryValidator() => this.RuleFor(x => x.Id).GreaterThan(0);
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            this.RuleFor(x => x.Id).GreaterThan(0);
            this.RuleFor(x => x.PhysicalAddress).SetValidator(new AddressValidator()!);
        }
    }

    public class AddressValidator : AbstractValidator<Command.Address>
    {
        public AddressValidator()
        {
            this.RuleFor(x => x.CountryCode).NotEmpty();
            this.RuleFor(x => x.ProvinceCode).NotEmpty();
            this.RuleFor(x => x.Street).NotEmpty();
            this.RuleFor(x => x.City).NotEmpty();
            this.RuleFor(x => x.Postal).NotEmpty();
        }
    }

    public class QueryHandler : IQueryHandler<Query, IDomainResult<Command>>
    {
        private readonly PidpDbContext context;
        private readonly IMapper mapper;

        public QueryHandler(PidpDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IDomainResult<Command>> HandleAsync(Query query)
        {
            var setting = await this.context.Parties
                .Where(party => party.Id == query.Id)
                .ProjectTo<Command>(this.mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            if (setting == null)
            {
                return DomainResult.NotFound<Command>();
            }

            return DomainResult.Success(setting);
        }
    }

    public class CommandHandler : ICommandHandler<Command, IDomainResult>
    {
        private readonly PidpDbContext context;

        public CommandHandler(PidpDbContext context) => this.context = context;

        public async Task<IDomainResult> HandleAsync(Command command)
        {
            var party = await this.context.Parties
                .Include(party => party.Facility)
                .SingleOrDefaultAsync(party => party.Id == command.Id);

            if (party == null)
            {
                return DomainResult.NotFound();
            }

            party.JobTitle = command.JobTitle;

            if (party.Facility == null)
            {
                party.Facility = new Facility();
            }

            party.Facility.Name = command.FacilityName;

            if (command.PhysicalAddress == null)
            {
                party.Facility.PhysicalAddress = null;
            }
            else
            {
                party.Facility.PhysicalAddress = new FacilityAddress
                {
                    CountryCode = command.PhysicalAddress.CountryCode,
                    ProvinceCode = command.PhysicalAddress.ProvinceCode,
                    Street = command.PhysicalAddress.Street,
                    City = command.PhysicalAddress.City,
                    Postal = command.PhysicalAddress.Postal
                };
            }

            await this.context.SaveChangesAsync();
            return DomainResult.Success();
        }
    }
}
