namespace Pidp.Features.VerifiedEmails;

using DomainResults.Common;
using FluentValidation;
using HybridModelBinding;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using System.Text.Json.Serialization;

using Pidp.Data;

public class Verify
{
    public class Command : ICommand<IDomainResult<Model>>
    {
        [JsonIgnore]
        [HybridBindProperty(Source.Route)]
        public int PartyId { get; set; }
        public Guid Token { get; set; }
    }

    public class Model
    {
        public string Email { get; set; } = string.Empty;
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            this.RuleFor(x => x.PartyId).GreaterThan(0);
            this.RuleFor(x => x.Token).NotEmpty();
        }
    }

    public class CommandHandler(IClock clock, ILogger<CommandHandler> logger, PidpDbContext context) : ICommandHandler<Command, IDomainResult<Model>>
    {
        private readonly IClock clock = clock;
        private readonly ILogger<CommandHandler> logger = logger;
        private readonly PidpDbContext context = context;

        public async Task<IDomainResult<Model>> HandleAsync(Command command)
        {
            var verifiedEmail = await this.context.VerifiedEmails
                .Where(verifiedEmail => verifiedEmail.PartyId == command.PartyId
                    && verifiedEmail.Token == command.Token)
                .SingleOrDefaultAsync();

            if (verifiedEmail == null)
            {
                this.logger.LogVerifiedEmailNotFound(command.PartyId, command.Token);
                return DomainResult.NotFound<Model>();
            }

            if (!verifiedEmail.IsVerified)
            {
                verifiedEmail.VerifiedOn = this.clock.GetCurrentInstant();
                await this.context.SaveChangesAsync();
            }

            return DomainResult.Success(new Model { Email = verifiedEmail.Email });
        }
    }
}

public static partial class VerifiedEmailsVerifyLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Error, "Could not find Verified Email on Party {partyId} with Token {token}.")]
    public static partial void LogVerifiedEmailNotFound(this ILogger<Verify.CommandHandler> logger, int partyId, Guid token);
}
