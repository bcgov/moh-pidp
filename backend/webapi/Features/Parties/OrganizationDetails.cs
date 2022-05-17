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

        public OrganizationCode OrganizationCode { get; set; }
        public HealthAuthorityCode HealthAuthorityCode { get; set; }
        public string EmployeeIdentifier { get; set; } = string.Empty;
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
            this.RuleFor(x => x.OrganizationCode).NotEmpty().IsInEnum();
            this.RuleFor(x => x.HealthAuthorityCode).NotEmpty().IsInEnum();
            this.RuleFor(x => x.EmployeeIdentifier).NotEmpty();
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
            var orgDetails = await this.context.PartyOrgainizationDetails
                .Where(detail => detail.PartyId == query.PartyId)
                .ProjectTo<Command>(this.mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            return orgDetails ?? new Command { PartyId = query.PartyId };
        }
    }

    public class CommandHandler : ICommandHandler<Command>
    {
        private readonly PidpDbContext context;

        public CommandHandler(PidpDbContext context) => this.context = context;

        public async Task HandleAsync(Command command)
        {
            var org = await this.context.PartyOrgainizationDetails
                .SingleOrDefaultAsync(detail => detail.PartyId == command.PartyId);

            if (org == null)
            {
                org = new Models.PartyOrgainizationDetail
                {
                    PartyId = command.PartyId
                };
                this.context.PartyOrgainizationDetails.Add(org);
            }

            org.OrganizationCode = command.OrganizationCode;
            org.HealthAuthorityCode = command.HealthAuthorityCode;
            org.EmployeeIdentifier = command.EmployeeIdentifier;

            await this.context.SaveChangesAsync();
        }
    }
}
