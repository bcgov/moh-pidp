namespace Pidp.Features.CommonHandlers;

using static Pidp.Features.CommonHandlers.UpdateKeycloakAttributesHandler;
using Pidp.Infrastructure.HttpClients.Keycloak;

public class UpdateKeycloakAttributesHandler(IKeycloakAdministrationClient client, ILogger<UpdateKeycloakAttributesHandler> logger)
{
    public class UpdateKeycloakAttributes(Guid userId, Dictionary<string, string[]> attributes)
    {
        public Guid UserId { get; set; } = userId;
        public Dictionary<string, string[]> Attributes { get; set; } = attributes;

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

    private readonly IKeycloakAdministrationClient client = client;
    private readonly ILogger<UpdateKeycloakAttributesHandler> logger = logger;

    public async Task HandleAsync(UpdateKeycloakAttributes message)
    {
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

internal static partial class UpdateKeycloakAttributesHandlerLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Error, "Error when GETing the User {userId} from Keycloak.")]
    public static partial void LogGetKeycloakUserFailure(this ILogger<UpdateKeycloakAttributesHandler> logger, Guid userId);

    [LoggerMessage(2, LogLevel.Error, "Error when updating the Keycloak User {userId} with the attributes: {attributes}.")]
    public static partial void LogUpdateKeycloakUserFailure(this ILogger<UpdateKeycloakAttributesHandler> logger, Guid userId, Dictionary<string, string[]> attributes);
}
