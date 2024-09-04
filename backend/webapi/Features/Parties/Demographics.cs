namespace Pidp.Features.Parties;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using HybridModelBinding;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

using Pidp.Data;
using Pidp.Extensions;
using static Pidp.Features.CommonHandlers.UpdateKeycloakAttributesConsumer;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.BCProvider;
using Pidp.Models.DomainEvents;

public class Demographics
{
    public class Query : IQuery<Command>
    {
        public int Id { get; set; }
    }

    public class Command : ICommand
    {
        [JsonIgnore]
        [HybridBindProperty(Source.Route)]
        public int Id { get; set; }

        public string? PreferredFirstName { get; set; }
        public string? PreferredMiddleName { get; set; }
        public string? PreferredLastName { get; set; }

        public string? Email { get; set; }
        public string? Phone { get; set; }
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator() => this.RuleFor(x => x.Id).GreaterThan(0);
    }

    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            this.RuleFor(x => x.Id).GreaterThan(0);
            this.RuleFor(x => x.Email).NotEmpty().EmailAddress(); // TODO Custom email validation?
            this.RuleFor(x => x.Phone).NotEmpty();
            this.RuleFor(x => x.PreferredFirstName)
                .NotWhiteSpace()
                .NotEmpty()
                .When(x => !string.IsNullOrWhiteSpace(x.PreferredLastName), ApplyConditionTo.CurrentValidator)
                .WithMessage("The preferred first name is required when the preferred last name has been entered");
            this.RuleFor(x => x.PreferredMiddleName).NotWhiteSpace();
            this.RuleFor(x => x.PreferredLastName)
                .NotWhiteSpace()
                .NotEmpty()
                .When(x => !string.IsNullOrWhiteSpace(x.PreferredFirstName), ApplyConditionTo.CurrentValidator)
                .WithMessage("The preferred last name is required when the preferred first name has been entered");
        }
    }

    public class QueryHandler(IMapper mapper, PidpDbContext context) : IQueryHandler<Query, Command>
    {
        private readonly IMapper mapper = mapper;
        private readonly PidpDbContext context = context;

        public async Task<Command> HandleAsync(Query query)
        {
            return await this.context.Parties
                .Where(party => party.Id == query.Id)
                .ProjectTo<Command>(this.mapper.ConfigurationProvider)
                .SingleAsync();
        }
    }

    public class CommandHandler(PidpDbContext context) : ICommandHandler<Command>
    {
        private readonly PidpDbContext context = context;

        public async Task HandleAsync(Command command)
        {
            var party = await this.context.Parties
                .Include(party => party.Credentials)
                .SingleAsync(party => party.Id == command.Id);

            if (party.Email != command.Email)
            {
                // TODO: update all credentials.
                party.DomainEvents.Add(new PartyEmailUpdated(party.Id, party.PrimaryUserId, command.Email!));
            }
            if (party.Phone != command.Phone)
            {
                party.DomainEvents.Add(new PartyPhoneUpdated(party.Id, party.Credentials.Select(credential => credential.UserId), command.Phone!));
            }

            party.PreferredFirstName = command.PreferredFirstName?.Trim();
            party.PreferredMiddleName = command.PreferredMiddleName?.Trim();
            party.PreferredLastName = command.PreferredLastName?.Trim();
            party.Email = command.Email;
            party.Phone = command.Phone;

            await this.context.SaveChangesAsync();
        }
    }

    public class PartyEmailUpdatedHandler(IBus bus) : INotificationHandler<PartyEmailUpdated>
    {
        private readonly IBus bus = bus;

        public async Task Handle(PartyEmailUpdated notification, CancellationToken cancellationToken)
        {
            // TODO: replace by pushing a generic Update BC Provider Attribute message.
            await this.bus.Publish(notification, cancellationToken);

            await this.bus.Publish(UpdateKeycloakAttributes.FromUpdateAction(notification.UserId, user => user.SetPidpEmail(notification.NewEmail)), cancellationToken);
        }
    }

    public class PartyEmailUpdatedBcProviderConsumer(
        IBCProviderClient client,
        PidpConfiguration config,
        PidpDbContext context,
        ILogger<PartyEmailUpdatedBcProviderConsumer> logger) : IConsumer<PartyEmailUpdated>
    {
        private readonly IBCProviderClient client = client;
        private readonly PidpDbContext context = context;
        private readonly string bcProviderClientId = config.BCProviderClient.ClientId;
        private readonly ILogger<PartyEmailUpdatedBcProviderConsumer> logger = logger;

        public async Task Consume(ConsumeContext<PartyEmailUpdated> context)
        {
            var bcProviderId = await this.context.Credentials
                .Where(credential => credential.PartyId == context.Message.PartyId
                    && credential.IdentityProvider == IdentityProviders.BCProvider)
                .Select(credential => credential.IdpId)
                .SingleOrDefaultAsync();

            if (string.IsNullOrEmpty(bcProviderId))
            {
                return;
            }

            var bcProviderAttributes = new BCProviderAttributes(this.bcProviderClientId).SetPidpEmail(context.Message.NewEmail);
            if (!await this.client.UpdateAttributes(bcProviderId, bcProviderAttributes.AsAdditionalData()))
            {
                this.logger.LogBCProviderEmailUpdateFailed(context.Message.UserId);
                throw new InvalidOperationException("Error Comunicating with Azure AD");
            }
        }
    }

    public class PartyPhoneUpdatedHandler(IBus bus) : INotificationHandler<PartyPhoneUpdated>
    {
        private readonly IBus bus = bus;

        public async Task Handle(PartyPhoneUpdated notification, CancellationToken cancellationToken)
        {
            foreach (var userId in notification.UserIds)
            {
                await this.bus.Publish(UpdateKeycloakAttributes.FromUpdateAction(userId, user => user.SetPidpPhone(notification.NewPhone)), cancellationToken);
            }
        }
    }
}

public static partial class PartyEmailUpdatedBcProviderConsumerLoggingExtensions
{
    [LoggerMessage(2, LogLevel.Error, "Error when updating the email to User #{userId} in Azure AD.")]
    public static partial void LogBCProviderEmailUpdateFailed(this ILogger<Demographics.PartyEmailUpdatedBcProviderConsumer> logger, Guid userId);
}
