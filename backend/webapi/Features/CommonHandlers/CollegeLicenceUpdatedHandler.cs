namespace Pidp.Features.CommonHandlers;

using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;

using Pidp.Data;
using static Pidp.Features.CommonHandlers.UpdateKeycloakAttributesConsumer;
using Pidp.Models.DomainEvents;
using Pidp.Infrastructure.HttpClients.Plr;

public class UpdateKeycloakAfterCollegeLicenceUpdated : INotificationHandler<CollegeLicenceUpdated>
{
    private readonly IBus bus;
    private readonly ILogger<UpdateKeycloakAfterCollegeLicenceUpdated> logger;
    private readonly IPlrClient plrClient;
    private readonly PidpDbContext context;

    public UpdateKeycloakAfterCollegeLicenceUpdated(
        IBus bus,
        ILogger<UpdateKeycloakAfterCollegeLicenceUpdated> logger,
        IPlrClient plrClient,
        PidpDbContext context)
    {
        this.bus = bus;
        this.logger = logger;
        this.plrClient = plrClient;
        this.context = context;
    }

    public async Task Handle(CollegeLicenceUpdated notification, CancellationToken cancellationToken)
    {
        // If this domain event is raised due to a newly found CPN or newly created credential, the CPN / UserId will be uncommitted in the context.
        // We must fetch the whole Party model (rather than .Select() a smaller model) to ensure the uncommited changes are seen here.
        var party = await this.context.Parties
            .Include(party => party.Credentials)
            .Where(party => party.Id == notification.PartyId)
            .SingleAsync(cancellationToken);

        var records = await this.plrClient.GetRecordsAsync(party.Cpn);
        if (records == null || !records.Any())
        {
            // TODO: error handling
            this.logger.LogPlrError(notification.PartyId, party.Cpn);
            return;
        }

        foreach (var userId in party.Credentials.Select(credenial => credenial.UserId))
        {
            await this.bus.Publish(UpdateKeycloakAttributes.FromUpdateAction(userId, user => user.SetCollegeLicenceInformation(records)), cancellationToken);
        }
    }
}

public static partial class UpdateKeycloakAfterCollegeLicenceUpdatedLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Error, "Error when updating Party {partyId}'s College JSON in keycloak: error talking to PLR. CPN = {cpn}.")]
    public static partial void LogPlrError(this ILogger<UpdateKeycloakAfterCollegeLicenceUpdated> logger, int partyId, string? cpn);
}
