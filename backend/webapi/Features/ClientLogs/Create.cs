namespace Pidp.Features.ClientLogs;

using FluentValidation;
using Pidp.Data;
using Pidp.Models;

public class Create
{
    public class Command : ICommand
    {
        public string Message { get; set; } = string.Empty;

        public LogLevel LogLevel { get; set; }

        public string BrowserInformation { get; set; } = string.Empty;
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator(IHttpContextAccessor accessor)
        {
            var user = accessor?.HttpContext?.User;

            this.RuleFor(x => x.LogLevel).NotEmpty();
            this.RuleFor(x => x.Message).NotEmpty();
            this.RuleFor(x => x.BrowserInformation).NotEmpty();
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
                Message = command.Message,
                LogLevel = command.LogLevel,
                BrowserInformation = command.BrowserInformation
            };

            this.context.ClientLogs.Add(clientLog);

            await this.context.SaveChangesAsync();
        }
    }
}
