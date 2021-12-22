namespace Pidp.Features.Parties;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

using Pidp.Data;
using Pidp.Models;
using Pidp.Models.Lookups;

public class Demographics
{
    public class Query : IQuery<Command>
    {
        public int Id { get; set; }
    }

    public class Command : ICommand
    {
        public int Id { get; set; }

        public string? PreferredFirstName { get; set; }
        public string? PreferredMiddleName { get; set; }
        public string? PreferredLastName { get; set; }

        public string? Email { get; set; }
        public string? Phone { get; set; }

        public Address? MailingAddress { get; set; }

        public class Address
        {
            public CountryCode CountryCode { get; set; }
            public ProvinceCode ProvinceCode { get; set; }
            public string Street { get; set; } = string.Empty;
            public string City { get; set; } = string.Empty;
            public string Postal { get; set; } = string.Empty;
        }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            this.RuleFor(x => x.Id).NotEmpty();
            this.RuleFor(x => x.Email).NotEmpty().EmailAddress();
            this.RuleFor(x => x.Phone).NotEmpty();
            this.RuleFor(x => x.MailingAddress).SetValidator(new AddressValidator()!);
        }
    }

    public class AddressValidator : AbstractValidator<Command.Address>
    {
        public AddressValidator()
        {
            this.RuleFor(x => x.CountryCode).IsInEnum();
            this.RuleFor(x => x.ProvinceCode).IsInEnum();
            this.RuleFor(x => x.Street).NotEmpty();
            this.RuleFor(x => x.City).NotEmpty();
            this.RuleFor(x => x.Postal).NotEmpty();
        }
    }

    public class QueryHandler : IQueryHandler<Query, Command>
    {
        private readonly PidpDbContext context;
        private readonly IMapper mapper;

        public QueryHandler(PidpDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<Command> HandleAsync(Query query)
        {
            return await this.context.Parties
                .Where(party => party.Id == query.Id)
                .ProjectTo<Command>(this.mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }
    }

    public class CommandHandler : ICommandHandler<Command>
    {
        private readonly PidpDbContext context;

        public CommandHandler(PidpDbContext context) => this.context = context;

        public async Task HandleAsync(Command command)
        {
            var party = await this.context.Parties
                .Include(party => party.MailingAddress)
                .SingleOrDefaultAsync(party => party.Id == command.Id);

            if (party == null)
            {
                return;
            }

            party.PreferredFirstName = command.PreferredFirstName;
            party.PreferredMiddleName = command.PreferredMiddleName;
            party.PreferredLastName = command.PreferredLastName;
            party.Email = command.Email;
            party.Phone = command.Phone;

            if (command.MailingAddress == null)
            {
                party.MailingAddress = null;
            }
            else
            {
                party.MailingAddress = new PartyAddress
                {
                    CountryCode = command.MailingAddress.CountryCode,
                    ProvinceCode = command.MailingAddress.ProvinceCode,
                    Street = command.MailingAddress.Street,
                    City = command.MailingAddress.City,
                    Postal = command.MailingAddress.Postal
                };
            }

            await this.context.SaveChangesAsync();
        }
    }
}
