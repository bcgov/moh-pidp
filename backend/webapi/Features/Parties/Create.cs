namespace Pidp.Features.Parties;

using FluentValidation;
using MassTransit;
using MediatR;
using NodaTime;

using Pidp.Data;
using Pidp.Extensions;
using Pidp.Infrastructure.Auth;
using Pidp.Models;
using Pidp.Models.DomainEvents;
using static Pidp.Features.CommonHandlers.UpdateKeycloakAttributesConsumer;

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

    public class CommandHandler(PidpDbContext context) : ICommandHandler<Command, int>
    {
        private readonly PidpDbContext context = context;

        public async Task<int> HandleAsync(Command command)
        {
            var party = new Party
            {
                Birthdate = command.Birthdate,
                FirstName = command.FirstName,
                LastName = command.LastName,
            };
            party.Credentials.Add(new Credential
            {
                UserId = command.UserId,
                IdentityProvider = command.IdentityProvider,
                IdpId = command.IdpId,
            });

            if (command.IdentityProvider == IdentityProviders.BCServicesCard)
            {
                await party.GenerateOpId(this.context);
                party.DomainEvents.Add(new OpIdCreated(command.UserId, party.OpId!));
            }

            this.context.Parties.Add(party);

            await this.context.SaveChangesAsync();

            return party.Id;
        }
    }

    public class UpdateKeycloakWhenOpIdCreatedHandler(IBus bus) : INotificationHandler<OpIdCreated>
    {
        private readonly IBus bus = bus;

        public async Task Handle(OpIdCreated notification, CancellationToken cancellationToken) => await this.bus.Publish(UpdateKeycloakAttributes.FromUpdateAction(notification.UserId, user => user.SetOpId(notification.OpId)), cancellationToken);
    }
}
