namespace PlrIntake.Features.Records;

using DomainResults.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

using PlrIntake.Data;

public class StatusLog
{
    public class Query : IQuery<List<Model>>
    {
        public int Limit { get; set; } = 10;
    }

    public class Model
    {
        public int Id { get; set; }
        public string? NewStatusCode { get; set; }
        public string? NewStatusReasonCode { get; set; }
        public string? OldStatusCode { get; set; }
        public string? OldStatusReasonCode { get; set; }
        public bool SouldBeProcessed { get; set; }
        public string? Cpn { get; set; }
    }

    public class Command : ICommand<IDomainResult>
    {
        public int StatusChangeLogId { get; set; }
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator() => this.RuleFor(x => x.Limit).GreaterThan(0).LessThan(10000);
    }


    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator() => this.RuleFor(x => x.StatusChangeLogId).GreaterThan(0);
    }

    public class QueryHandler : IQueryHandler<Query, List<Model>>
    {
        private readonly PlrDbContext context;

        public QueryHandler(PlrDbContext context) => this.context = context;

        public async Task<List<Model>> HandleAsync(Query query)
        {
            return await this.context.StatusChageLogs
                .Where(log => log.SouldBeProcessed)
                .OrderBy(log => log.Created)
                .Take(query.Limit)
                .Select(query => new Model()
                {
                    Id = query.Id,
                    NewStatusCode = query.NewStatusCode,
                    NewStatusReasonCode = query.NewStatusReasonCode,
                    OldStatusCode = query.OldStatusCode,
                    OldStatusReasonCode = query.OldStatusReasonCode,
                    SouldBeProcessed = query.SouldBeProcessed,
                    Cpn = query.PlrRecord!.Cpn,
                })
                .ToListAsync();
        }
    }

    public class CommandHandler : ICommandHandler<Command, IDomainResult>
    {
        private readonly PlrDbContext context;

        public CommandHandler(PlrDbContext context) => this.context = context;

        public async Task<IDomainResult> HandleAsync(Command command)
        {
            var statusChangeLog = await this.context.StatusChageLogs
                .SingleOrDefaultAsync(detail => detail.Id == command.StatusChangeLogId);

            if (statusChangeLog == null)
            {
                return DomainResult.NotFound();
            }

            statusChangeLog.SouldBeProcessed = false;

            await this.context.SaveChangesAsync();
            return DomainResult.Success();
        }
    }
}
