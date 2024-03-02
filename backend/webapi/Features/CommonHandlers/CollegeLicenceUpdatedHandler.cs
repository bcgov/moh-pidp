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
        var party = await this.context.Parties
            .Include(party => party.Credentials)
            .Where(party => party.Id == notification.PartyId)
            .SingleAsync(cancellationToken);

        var records = await this.plrClient.GetRecordsAsync(party.Cpn);
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

        if (collegeLicenceInformationList != null)
        {
            foreach (var userId in party.Credentials.Select(credential => credential.UserId))
            {
                await this.keycloakClient.UpdateUser(userId, (user) => user.SetCollegeLicenceInformation(collegeLicenceInformationList));
            }
        }
    }
}
