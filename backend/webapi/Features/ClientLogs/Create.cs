namespace Pidp.Features.ClientLogs;

using FluentValidation;
using Pidp.Data;
using Pidp.Models;

public class Create
{
    public class Command : ICommand
    {

        public int Id { get; set; }

        public string Message { get; set; } = string.Empty;

        public LogLevel? LogType { get; set; }

        public string PageInformation { get; set; } = string.Empty;

    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator(IHttpContextAccessor accessor)
        {
            var user = accessor?.HttpContext?.User;

            this.RuleFor(x => x.Id).NotEmpty();
            this.RuleFor(x => x.LogType).NotEmpty();
            this.RuleFor(x => x.Message).NotEmpty();
            this.RuleFor(x => x.PageInformation).NotEmpty();
        }
    }

    public class CommandHandler : ICommandHandler<Command>
    {
        private readonly PidpDbContext context;

        public CommandHandler(PidpDbContext context) => this.context = context;

        public async Task HandleAsync(Command command)
        {
            var clientLog = new ClientLog
            {
                Id = command.Id,
                Message = command.Message,
                LogType = command.LogType,
                PageInformation = command.PageInformation
            };

            this.context.ClientLogs.Add(clientLog);

            await this.context.SaveChangesAsync();
        }

    }

}
