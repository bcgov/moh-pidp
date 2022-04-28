namespace Pidp.Features.Parties;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using HybridModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

using Pidp.Data;
using Pidp.Features;
using Pidp.Models;

public class WorkSetting
{
    public class Query : IQuery<Command>
    {
        public int Id { get; set; }
    }

    public class Command : ICommand
    {
        [JsonIgnore]
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

    public class QueryHandler : IQueryHandler<Query, Command>
    {
        private readonly IMapper mapper;
        private readonly PidpDbContext context;

        public QueryHandler(IMapper mapper, PidpDbContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public async Task<Command> HandleAsync(Query query)
        {
            return await this.context.Parties
                .Where(party => party.Id == query.Id)
                .ProjectTo<Command>(this.mapper.ConfigurationProvider)
                .SingleAsync();
        }
    }

    public class CommandHandler : ICommandHandler<Command>
    {
        private readonly PidpDbContext context;

        public CommandHandler(PidpDbContext context) => this.context = context;

        public async Task HandleAsync(Command command)
        {
            var party = await this.context.Parties
                .Include(party => party.Facility)
                .SingleAsync(party => party.Id == command.Id);

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
        }
    }
}
