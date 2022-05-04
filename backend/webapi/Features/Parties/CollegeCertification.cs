namespace Pidp.Features.Parties;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using HybridModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

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

    public class Command : ICommand
    {
        [JsonIgnore]
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
            var cert = await this.context.PartyCertifications
                .Where(certification => certification.PartyId == query.PartyId)
                .ProjectTo<Command>(this.mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            return cert ?? new Command { PartyId = query.PartyId };
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
                .SingleAsync(party => party.Id == command.PartyId);

            if (party.PartyCertification == null)
            {
                party.PartyCertification = new PartyCertification();
            }

            party.PartyCertification.CollegeCode = command.CollegeCode;
            party.PartyCertification.LicenceNumber = command.LicenceNumber;
            party.PartyCertification.Ipc = party.Birthdate.HasValue
                ? await this.client.GetPlrRecord(command.CollegeCode, command.LicenceNumber, party.Birthdate.Value)
                : null;

            await this.context.SaveChangesAsync();
        }
    }
}
