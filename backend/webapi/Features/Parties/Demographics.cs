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
using Pidp.Features.CommonHandlers;
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

    public class QueryHandler : IQueryHandler<Query, Command>
    {
        private readonly IMapper mapper;
        private readonly PidpDbContext context;

        public QueryHandler(IMapper mapper, PidpDbContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public async Task<Command> HandleAsync(Query query)
        {
            return await this.context.Parties
                .Where(party => party.Id == query.Id)
                .ProjectTo<Command>(this.mapper.ConfigurationProvider)
                .SingleAsync();
        }
    }

    public class CommandHandler : ICommandHandler<Command>
    {
        private readonly PidpDbContext context;

        public CommandHandler(PidpDbContext context) => this.context = context;

        public async Task HandleAsync(Command command)
        {
            var party = await this.context.Parties
                .Include(party => party.Credentials)
                .SingleAsync(party => party.Id == command.Id);

            if (party.Email != null
                && party.Email != command.Email)
            {
                party.DomainEvents.Add(new PartyEmailUpdated(party.Id, party.PrimaryUserId, command.Email!));
            }

            party.PreferredFirstName = command.PreferredFirstName?.Trim();
            party.PreferredMiddleName = command.PreferredMiddleName?.Trim();
            party.PreferredLastName = command.PreferredLastName?.Trim();
            party.Email = command.Email;
            party.Phone = command.Phone;

            await this.context.SaveChangesAsync();
        }
    }

    public class PartyEmailUpdatedHandler : INotificationHandler<PartyEmailUpdated>
    {
        private readonly IBus bus;

        public PartyEmailUpdatedHandler(IBus bus) => this.bus = bus;

        public async Task Handle(PartyEmailUpdated notification, CancellationToken cancellationToken)
        {
            // TODO: relace by pushing a generic Update BC Provider Attribute message.
            await this.bus.Publish(notification, cancellationToken);

            await this.bus.Publish(new UpdateKeycloakAttributes(notification.UserId, user => user.SetPidpEmail(notification.NewEmail)), cancellationToken);
        }
    }

    public class PartyEmailUpdatedBcProviderConsumer : IConsumer<PartyEmailUpdated>
    {
        private readonly IBCProviderClient client;
        private readonly PidpDbContext context;
        private readonly string bcProviderClientId;
        private readonly ILogger<PartyEmailUpdatedBcProviderConsumer> logger;

        public PartyEmailUpdatedBcProviderConsumer(
            IBCProviderClient client,
            PidpConfiguration config,
            PidpDbContext context,
            ILogger<PartyEmailUpdatedBcProviderConsumer> logger)
        {
            this.client = client;
            this.context = context;
            this.bcProviderClientId = config.BCProviderClient.ClientId;
            this.logger = logger;
        }

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
}

public static partial class PartyEmailUpdatedBcProviderConsumerLoggingExtensions
{
    [LoggerMessage(2, LogLevel.Error, "Error when updating the email to User #{userId} in Azure AD.")]
    public static partial void LogBCProviderEmailUpdateFailed(this ILogger logger, Guid userId);
}
