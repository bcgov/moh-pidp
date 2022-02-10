namespace Pidp.Features.Parties;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using DomainResults.Common;
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
    public class Query : IQuery<IDomainResult<Command>>
    {
        public int PartyId { get; set; }
    }

    public class Command : ICommand<IDomainResult>
    {
        [HybridBindProperty(Source.Route)]
        public int PartyId { get; set; }
        public CollegeCode CollegeCode { get; set; }
        public string LicenceNumber { get; set; } = string.Empty;
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator() => this.RuleFor(x => x.PartyId).GreaterThan(0);
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            this.RuleFor(x => x.PartyId).GreaterThan(0);
            this.RuleFor(x => x.CollegeCode).IsInEnum();
            this.RuleFor(x => x.LicenceNumber).NotEmpty();
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
            var partyExists = await this.context.Parties
                .AnyAsync(party => party.Id == query.PartyId);

            if (!partyExists)
            {
                return DomainResult.NotFound<Command>();
            }

            var cert = await this.context.PartyCertifications
                .Where(certification => certification.PartyId == query.PartyId)
                .ProjectTo<Command>(this.mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            return DomainResult.Success(cert ?? new Command { PartyId = query.PartyId });
        }
    }

    public class CommandHandler : ICommandHandler<Command, IDomainResult>
    {
        private readonly IPlrClient client;
        private readonly PidpDbContext context;

        public CommandHandler(IPlrClient client, PidpDbContext context)
        {
            this.client = client;
            this.context = context;
        }

        public async Task<IDomainResult> HandleAsync(Command command)
        {
            var party = await this.context.Parties
                .Include(party => party.PartyCertification)
                .SingleOrDefaultAsync(party => party.Id == command.PartyId);

            if (party == null)
            {
                return DomainResult.NotFound();
            }

            if (party.PartyCertification == null)
            {
                party.PartyCertification = new PartyCertification();
            }

            party.PartyCertification.CollegeCode = command.CollegeCode;
            party.PartyCertification.LicenceNumber = command.LicenceNumber;
            party.PartyCertification.Ipc = await this.client.GetPlrRecord(command.CollegeCode, command.LicenceNumber, party.Birthdate);

            await this.context.SaveChangesAsync();
            return DomainResult.Success();
        }
    }
}
