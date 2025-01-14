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

        public string? AdditionalInformation { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            this.RuleFor(x => x.Message).NotEmpty();
            this.RuleFor(x => x.LogLevel).NotEmpty().IsInEnum();
        }
    }

    public class CommandHandler(PidpDbContext context) : ICommandHandler<Command>
    {
        private readonly PidpDbContext context = context;

        public async Task HandleAsync(Command command)
        {
            var clientLog = new ClientLog
            {
                Message = command.Message,
                LogLevel = command.LogLevel,
                AdditionalInformation = command.AdditionalInformation
            };

            this.context.ClientLogs.Add(clientLog);

            await this.context.SaveChangesAsync();
        }
    }
}
