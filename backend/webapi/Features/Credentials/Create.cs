namespace Pidp.Features.Credentials;

using FluentValidation;

using Pidp.Data;
using Pidp.Extensions;
using Pidp.Models;

public class Create
{
    public class Command : ICommand<int>
    {
        public Guid UserId { get; set; }
        // public string? Hpdid { get; set; }
        public string? IdpId { get; set; }

        public CredentialType CredentialType { get; set; }
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator(IHttpContextAccessor accessor)
        {
            var user = accessor?.HttpContext?.User;

            this.RuleFor(x => x.UserId).NotEmpty().Equal(user.GetUserId());

        }
    }

    public class CommandHandler : ICommandHandler<Command, int>
    {
        private readonly PidpDbContext context;

        public CommandHandler(PidpDbContext context) => this.context = context;

        public async Task<int> HandleAsync(Command command)
        {
            var credential = new Credential
            {
                UserId = command.UserId,
                IdpId = command.IdpId,
                CredentialType = command.CredentialType
            };

            this.context.Credentials.Add(credential);

            await this.context.SaveChangesAsync();

            return credential.Id;
        }
    }
}
