namespace Pidp.Features.Parties;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using HybridModelBinding;
using Microsoft.EntityFrameworkCore;

using Pidp.Data;
using Pidp.Features;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models;
using Pidp.Models.Lookups;

public class CollegeCertification
{
    public class Query : IQuery<Command>
    {
        public int PartyId { get; set; }
    }


    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator() => this.RuleFor(x => x.PartyId).NotEmpty();
    }

    public class Command : ICommand
    {
        [HybridBindProperty(Source.Route)]
        public int PartyId { get; set; }
        public CollegeCode CollegeCode { get; set; }
        public string LicenceNumber { get; set; } = string.Empty;
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            this.RuleFor(x => x.PartyId).NotEmpty();
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
                .Where(certification => certification.PartyId == query.PartyId)
                .ProjectTo<Command>(this.mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }
    }

    public class CommandHandler : ICommandHandler<Command>
    {
        private readonly IPlrClient client;
        private readonly PidpDbContext context;

        public CommandHandler(IPlrClient client, PidpDbContext context)
        {
            this.client = client;
            this.context = context;
        }

        public async Task HandleAsync(Command command)
        {
            var party = await this.context.Parties
                .Include(party => party.PartyCertification)
                .SingleOrDefaultAsync(party => party.Id == command.PartyId);

            if (party == null)
            {
                // TODO 404
                return;
            }

            if (party.PartyCertification == null)
            {
                party.PartyCertification = new PartyCertification();
            }

            party.PartyCertification.CollegeCode = command.CollegeCode;
            party.PartyCertification.LicenceNumber = command.LicenceNumber;
            party.PartyCertification.Ipc = await this.client.GetPlrRecord(command.LicenceNumber, command.CollegeCode, party.Birthdate);

            await this.context.SaveChangesAsync();
        }
    }
}
