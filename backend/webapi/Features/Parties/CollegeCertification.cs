namespace Pidp.Features.Parties;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Pidp.Data;
using Pidp.Features;
using Pidp.Models;
using Pidp.Models.Lookups;

public class CollegeCertification
{
    public class Query : IQuery<Command>
    {
        public int Id { get; set; }
    }

    public class Command : ICommand
    {
        public int Id { get; set; }
        public CollegeCode CollegeCode { get; set; }
        public string LicenceNumber { get; set; } = string.Empty;
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            this.RuleFor(x => x.Id).NotEmpty();
            this.RuleFor(x => x.CollegeCode).IsInEnum();
            this.RuleFor(x => x.LicenceNumber).NotEmpty();
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
            return await this.context.PartyCertifications
                .Where(certification => certification.Id == query.Id)
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
            var partyCertification = await this.context.PartyCertifications
                .SingleOrDefaultAsync(certification => certification.PartyId == command.Id);

            if (partyCertification == null)
            {
                var party = await this.context.Parties.FindAsync(command.Id);
                partyCertification = new PartyCertification
                {
                    Party = party,
                };
                this.context.PartyCertifications.Add(partyCertification);
            }

            partyCertification.CollegeCode = command.CollegeCode;
            partyCertification.LicenceNumber = command.LicenceNumber;

            await this.context.SaveChangesAsync();
        }
    }
}
