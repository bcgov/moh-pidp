namespace PlrIntake.Features.Records;

using FluentValidation;
using Microsoft.EntityFrameworkCore;

using PlrIntake.Data;
using PlrIntake.Extensions;

public class Search
{
    public class Command : ICommand<List<string>>
    {
        public string CollegeId { get; set; } = string.Empty;
        public DateTime Birthdate { get; set; }
        public List<string> IdentifierTypes { get; set; } = new();
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            this.RuleFor(x => x.CollegeId).NotEmpty();
            this.RuleFor(x => x.Birthdate).NotEmpty();
            this.RuleFor(x => x.IdentifierTypes).NotEmpty();
            this.RuleForEach(x => x.IdentifierTypes).NotEmpty();
        }
    }

    public class CommandHandler : ICommandHandler<Command, List<string>>
    {
        private readonly PlrDbContext context;

        public CommandHandler(PlrDbContext context) => this.context = context;

        public async Task<List<string>> HandleAsync(Command command)
        {
            return await this.context.PlrRecords
                .ExcludeDeleted()
                .Where(record => record.CollegeId!.TrimStart('0') == command.CollegeId.TrimStart('0')
                    && record.DateOfBirth!.Value.Date == command.Birthdate.Date
                    && command.IdentifierTypes.Contains(record.IdentifierType!))
                .Select(record => record.Cpn!) // All valid PLR records will have CPNs
                .ToListAsync();
        }
    }
}
