namespace Pidp.Features.Parties;

using FluentValidation;
using NanoidDotNet;
using NodaTime;

using Pidp.Data;
using Pidp.Extensions;
using Pidp.Infrastructure.Auth;
using Pidp.Models;

public class Create
{
    public class Command : ICommand<int>
    {
        public Guid UserId { get; set; }
        public string IdentityProvider { get; set; } = string.Empty;
        public string IdpId { get; set; } = string.Empty;
        public LocalDate? Birthdate { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string OpId { get; set; } = string.Empty;
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator(IHttpContextAccessor accessor)
        {
            var user = accessor?.HttpContext?.User;

            this.RuleFor(x => x.UserId).NotEmpty().Equal(user.GetUserId());
            this.RuleFor(x => x.IdentityProvider).NotEmpty().Equal(user.GetIdentityProvider())
                .NotEqual(IdentityProviders.BCProvider).WithMessage("Bc Provider cannot be used to create a Party");
            this.RuleFor(x => x.IdpId).NotEmpty().Equal(user.GetIdpId());
            this.RuleFor(x => x.FirstName).NotEmpty().MatchesUserClaim(user, Claims.GivenName);
            this.RuleFor(x => x.LastName).NotEmpty().MatchesUserClaim(user, Claims.FamilyName);

            this.When(x => x.IdentityProvider == IdentityProviders.BCServicesCard, () => this.RuleFor(x => x.Birthdate).NotEmpty().Equal(user?.GetBirthdate()).WithMessage("Must match the \"birthdate\" Claim on the current User"))
                .Otherwise(() => this.RuleFor(x => x.Birthdate).Empty());
        }
    }

    public class CommandHandler : ICommandHandler<Command, int>
    {
        private readonly PidpDbContext context;
        private readonly ILogger logger;

        public CommandHandler(PidpDbContext context, ILogger<CommandHandler> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public async Task<int> HandleAsync(Command command)
        {
            var party = new Party
            {
                Birthdate = command.Birthdate,
                FirstName = command.FirstName,
                LastName = command.LastName,
                OpId = command.IdentityProvider == IdentityProviders.BCServicesCard ? Nanoid.Generate("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ") : string.Empty,
            };
            party.Credentials.Add(new Credential
            {
                UserId = command.UserId,
                IdentityProvider = command.IdentityProvider,
                IdpId = command.IdpId,
            });

            this.context.Parties.Add(party);

            bool saveFailed;
            var maxAttempts = 100;
            do
            {
                saveFailed = false;
                try
                {
                    await this.context.SaveChangesAsync();
                }
                catch
                {
                    saveFailed = true;
                    maxAttempts--;
                    if (maxAttempts > 0)
                    {
                        party.OpId = Nanoid.Generate("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ");
                    }
                    else
                    {
                        this.logger.LogOpIdGenerationFailed(party.Id);
                        throw new InvalidOperationException("Error Generating OPID.");
                    }
                }
            } while (saveFailed && maxAttempts > 0);

            return party.Id;
        }
    }
}

public static partial class PartyCreateLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Warning, "OPID generation failed for {partyId}.")]
    public static partial void LogOpIdGenerationFailed(this ILogger logger, int partyId);
}
