namespace Pidp.Features.Parties;

using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Pidp.Data;
using Pidp.Features;
using Pidp.Models;
using Pidp.Models.Lookups;

public class WorkSetting
{
    public class Query : IQuery<Command>
    {
        public int Id { get; set; }
    }

    public class Command : ICommand
    {
        public int Id { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string FacilityName { get; set; } = string.Empty;

        public Address? FacilityAddress { get; set; }

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
            this.RuleFor(x => x.FacilityAddress).SetValidator(new AddressValidator()!);
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

    public class CommandHandler : ICommandHandler<Command>
    {
        private readonly PidpDbContext context;

        public CommandHandler(PidpDbContext context) => this.context = context;

        public async Task HandleAsync(Command command)
        {
            var party = await this.context.Parties
                .Include(party => party.Facility)
                .SingleOrDefaultAsync(party => party.Id == command.Id);

            if (party == null)
            {
                return;
            }

            party.JobTitle = command.JobTitle;

            // if (party.Facility == null)
            // {
            //     var partyCertification = new PartyCertification
            //     {
            //         Party = party,
            //     };
            //     this.context.PartyCertifications.Add(partyCertification);
            // }
            // else
            // {

            // }

            await this.context.SaveChangesAsync();
        }
    }
}
