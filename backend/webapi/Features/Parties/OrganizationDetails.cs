namespace Pidp.Features.Parties;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using HybridModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

using Pidp.Data;
using Pidp.Models.Lookups;

public class OrganizationDetails
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

        public OrganizationCode OrganizationType { get; set; }
        public HealthAuthorityCode HealthAuthorityType { get; set; }
        public string EmployeeId { get; set; } = string.Empty;
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
            this.RuleFor(x => x.OrganizationType).NotEmpty().IsInEnum();
            this.RuleFor(x => x.HealthAuthorityType).NotEmpty().IsInEnum();
            this.RuleFor(x => x.EmployeeId).NotEmpty();
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
            // TODO what should this table be?
            // return await this.context.???
            //     .Where(??? => ???.PartyId == query.PartyId)
            // TODO add to mapping profile
            //     .ProjectTo<Command>(this.mapper.ConfigurationProvider)
            //     .SingleAsync();
        }
    }

    public class CommandHandler : ICommandHandler<Command>
    {
        private readonly PidpDbContext context;

        public CommandHandler(PidpDbContext context) => this.context = context;

        public async Task HandleAsync(Command command)
        {
            // TODO add to parties or separate out into own table?
            // var ??? = await this.context.???
            //     .SingleAsync(??? => ???.PartyId == command.PartyId);

            // ???.OrganizationType = command.OrganizationType;
            // ???.HealthAuthorityType = command.HealthAuthorityType;
            // ???.EmployeeId = command.EmployeeId;

            await this.context.SaveChangesAsync();
        }
    }
}
