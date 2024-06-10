namespace Pidp.Features.CommonHandlers;

using MassTransit;

using static Pidp.Features.CommonHandlers.UpdateKeycloakAttributesConsumer;
using Pidp.Infrastructure.HttpClients.Keycloak;

public class UpdateKeycloakAttributesConsumer : IConsumer<UpdateKeycloakAttributes>
{
    public class UpdateKeycloakAttributes
    {
        public Guid UserId { get; set; }
        public Dictionary<string, string[]> Attributes { get; set; }

        public UpdateKeycloakAttributes(Guid userId, Dictionary<string, string[]> attributes)
        {
            this.UserId = userId;
            this.Attributes = attributes;
        }

        /// <summary>
        /// Will only update Attributes; will not update Email or any other Properties modified on the User Representation.
        /// </summary>
        public static UpdateKeycloakAttributes FromUpdateAction(Guid userId, Action<UserRepresentation> updateAction)
        {
            UserRepresentation userRep = new();
            updateAction(userRep);

            return new UpdateKeycloakAttributes(userId, userRep.Attributes);
        }
    }

    private readonly IKeycloakAdministrationClient client;
    private readonly ILogger<UpdateKeycloakAttributesConsumer> logger;

    public UpdateKeycloakAttributesConsumer(IKeycloakAdministrationClient client, ILogger<UpdateKeycloakAttributesConsumer> logger)
    {
        this.client = client;
        this.logger = logger;
    }

    public async Task Consume(ConsumeContext<UpdateKeycloakAttributes> context)
    {
        var message = context.Message;
        var userRep = await this.client.GetUser(message.UserId);
        if (userRep == null)
        {
            this.logger.LogGetKeycloakUserFailure(message.UserId);
            throw new InvalidOperationException("Error when GETing User from Keycloak");
        }

        userRep.SetAttributes(message.Attributes);

        if (!await this.client.UpdateUser(message.UserId, userRep))
        {
            this.logger.LogUpdateKeycloakUserFailure(message.UserId, userRep.Attributes);
            throw new InvalidOperationException("Error when updating Keycloak User");
        }
    }
}

internal static partial class UpdateKeycloakAttributesConsumerLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Error, "Error when GETing the User {userId} from Keycloak.")]
    public static partial void LogGetKeycloakUserFailure(this ILogger<UpdateKeycloakAttributesConsumer> logger, Guid userId);

    [LoggerMessage(2, LogLevel.Error, "Error when updating the Keycloak User {userId} with the attributes: {attributes}.")]
    public static partial void LogUpdateKeycloakUserFailure(this ILogger<UpdateKeycloakAttributesConsumer> logger, Guid userId, Dictionary<string, string[]> attributes);
}
