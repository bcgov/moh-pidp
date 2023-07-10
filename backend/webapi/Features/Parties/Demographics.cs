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
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.BCProvider;
using Pidp.Infrastructure.HttpClients.Keycloak;
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

            party.PreferredFirstName = command.PreferredFirstName;
            party.PreferredMiddleName = command.PreferredMiddleName;
            party.PreferredLastName = command.PreferredLastName;
            party.Email = command.Email;
            party.Phone = command.Phone;

            await this.context.SaveChangesAsync();
        }
    }

    public class PartyEmailUpdatedHandler : INotificationHandler<PartyEmailUpdated>
    {
        private readonly IBus bus;
        private readonly IBCProviderClient bcProviderClient;
        private readonly PidpDbContext context;
        private readonly string bcProviderClientId;

        public PartyEmailUpdatedHandler(
            IBCProviderClient bcProviderClient,
            IBus bus,
            PidpConfiguration config,
            PidpDbContext context)
        {
            this.bcProviderClient = bcProviderClient;
            this.keycloakClient = keycloakClient;
            this.context = context;
            this.bcProviderClientId = config.BCProviderClient.ClientId;
            this.bus = bus;
        }

        public async Task Handle(PartyEmailUpdated notification, CancellationToken cancellationToken)
        {
            var bcProviderId = await this.context.Credentials
                .Where(credential => credential.PartyId == notification.PartyId
                    && credential.IdentityProvider == IdentityProviders.BCProvider)
                .Select(credential => credential.IdpId)
                .SingleOrDefaultAsync(cancellationToken);

            if (string.IsNullOrEmpty(bcProviderId))
            {
                return;
            }

            var bcProviderAttributes = new BCProviderAttributes(this.bcProviderClientId).SetPidpEmail(notification.NewEmail);
            await this.client.UpdateAttributes(bcProviderId, bcProviderAttributes.AsAdditionalData());
            // TODO: Log Failure?

            var endpoint = await this.bus.GetSendEndpoint(new Uri("queue:party-email-updated"));
            await endpoint.Send(notification, cancellationToken);
        }
    }

    public class PartyEmailUpdatedConsumer : IConsumer<PartyEmailUpdated>
    {
        private readonly IKeycloakAdministrationClient keycloakClient;
        private readonly ILogger<PartyEmailUpdatedConsumer> logger;

        public PartyEmailUpdatedConsumer(
            IKeycloakAdministrationClient keycloakClient,
            ILogger<PartyEmailUpdatedConsumer> logger)
        {
            this.keycloakClient = keycloakClient;
            this.logger = logger;
        }

        public async Task Consume(ConsumeContext<PartyEmailUpdated> context)
        {
            if (!await this.keycloakClient.UpdateUser(context.Message.userId, (user) => user.SetPidpEmail(context.Message.NewEmail)))
            {
                this.logger.LogKeycloakEmailUpdateFailed(context.Message.userId);
                throw new InvalidOperationException("Error Comunicating with Keycloak");
            }
        }
    }

    public class PartyEmailUpdatedConsumerDefinition : ConsumerDefinition<PartyEmailUpdatedConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
            IConsumerConfigurator<PartyEmailUpdatedConsumer> consumerConfigurator)
        {
            endpointConfigurator.UseMessageRetry(r => r.Incremental(5, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(30)));
            endpointConfigurator.UseInMemoryOutbox();
        }
    }
}

public static partial class PartyEmailUpdatedConsumerLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Error, "Error when updating the email to User #{userId}.")]
    public static partial void LogKeycloakEmailUpdateFailed(this ILogger logger, Guid userId);
}
