namespace Pidp.Features.CommonHandlers;

using MediatR;
using Microsoft.EntityFrameworkCore;
using Pidp.Data;
using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models.DomainEvents;

public class UpdateKeycloakAfterCollegeLicenceUpdated : INotificationHandler<CollegeLicenceUpdated>
{
    private readonly IKeycloakAdministrationClient keycloakClient;
    private readonly IPlrClient plrClient;

    private readonly PidpDbContext context;

    public UpdateKeycloakAfterCollegeLicenceUpdated(
        IKeycloakAdministrationClient keycloakClient,
        IPlrClient plrClient,
        PidpDbContext context)
    {
        this.keycloakClient = keycloakClient;
        this.plrClient = plrClient;
        this.context = context;
    }

    public async Task Handle(CollegeLicenceUpdated notification, CancellationToken cancellationToken)
    {
        var partyCpn = await this.context.Parties
            .Where(party => party.Id == notification.PartyId)
            .Select(party => party.Cpn)
            .SingleAsync(cancellationToken);

        var records = await this.plrClient.GetRecordsAsync(partyCpn);
        var collegeLicenceInformationList = new List<CollegeLicenceInformation>();

        foreach (var record in records ?? Enumerable.Empty<PlrRecord>())
        {
            var collegeLicenceInformation = new CollegeLicenceInformation
            {
                ProviderRoleType = record.ProviderRoleType,
                StatusCode = record.StatusCode,
                StatusReasonCode = record.StatusReasonCode,
                MspId = record.MspId,
                CollegeId = record.CollegeId
            };
            collegeLicenceInformationList.Add(collegeLicenceInformation);
        }

        var userId = notification.UserId;

        await this.keycloakClient.UpdateUserCollegeLicenceInformation(userId, collegeLicenceInformationList);
    }

}
