namespace Pidp.Features.Parties;

using FluentValidation;
using Microsoft.EntityFrameworkCore;

using Pidp.Data;

public partial class UserAccessAgreement
{
    public class Command : ICommand<Model>
    {
        public int Id { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator() => this.RuleFor(x => x.Id).GreaterThan(0);
    }

    public class CommandHandler : ICommandHandler<Command, Model>
    {
        private readonly PidpDbContext context;

        public CommandHandler(PidpDbContext context) => this.context = context;

        public async Task<Model> HandleAsync(Command command)
        {
            Console.WriteLine($"User access agreement has been accepted. PartyId = {command.Id}");
            return await Task.Run(() => new Model());
        }
    }

    public partial class Model
    {
    }
}
