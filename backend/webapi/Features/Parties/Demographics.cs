namespace Pidp.Features.Parties;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using DomainResults.Common;
using FluentValidation;
using HybridModelBinding;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

using Pidp.Data;

public class Demographics
{
    public class Query : IQuery<IDomainResult<Command>>
    {
        public int Id { get; set; }
    }

    public class Command : ICommand<IDomainResult>
    {
        [JsonIgnore]
        [HybridBindProperty(Source.Route)]
        public int Id { get; set; }

        public string? PreferredFirstName { get; set; }
        public string? PreferredMiddleName { get; set; }
        public string? PreferredLastName { get; set; }

        public string? Email { get; set; }
        public string? Phone { get; set; }
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
            this.RuleFor(x => x.Email).NotEmpty().EmailAddress(); // TODO Custom email validation?
            this.RuleFor(x => x.Phone).NotEmpty();
        }
    }

    public class QueryHandler : IQueryHandler<Query, IDomainResult<Command>>
    {
        private readonly IMapper mapper;
        private readonly PidpDbContext context;

        public QueryHandler(IMapper mapper, PidpDbContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public async Task<IDomainResult<Command>> HandleAsync(Query query)
        {
            var demographic = await this.context.Parties
                .Where(party => party.Id == query.Id)
                .ProjectTo<Command>(this.mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

            if (demographic == null)
            {
                return DomainResult.NotFound<Command>();
            }

            return DomainResult.Success(demographic);
        }
    }

    public class CommandHandler : ICommandHandler<Command, IDomainResult>
    {
        private readonly PidpDbContext context;

        public CommandHandler(PidpDbContext context) => this.context = context;

        public async Task<IDomainResult> HandleAsync(Command command)
        {
            var party = await this.context.Parties
                .SingleOrDefaultAsync(party => party.Id == command.Id);

            if (party == null)
            {
                return DomainResult.NotFound();
            }

            party.PreferredFirstName = command.PreferredFirstName;
            party.PreferredMiddleName = command.PreferredMiddleName;
            party.PreferredLastName = command.PreferredLastName;
            party.Email = command.Email;
            party.Phone = command.Phone;

            await this.context.SaveChangesAsync();
            return DomainResult.Success();
        }
    }
}
