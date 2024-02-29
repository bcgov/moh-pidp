namespace MigrateCollegeLicenceJson;

using Microsoft.EntityFrameworkCore;
using Pidp.Data;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.Keycloak;

public class MigrateCollegeLicenceJsonService : IMigrateCollegeLicenceJsonService
{
    private readonly PidpDbContext context;
    private readonly IKeycloakAdministrationClient keycloakClient;
    private readonly IPlrClient plrClient;

    public MigrateCollegeLicenceJsonService(
        PidpDbContext context,
        IKeycloakAdministrationClient keycloakClient,
        IPlrClient plrClient)
    {
        this.context = context;
        this.keycloakClient = keycloakClient;
        this.plrClient = plrClient;
    }

    public async Task MigrateCollegeLicenceJsonAsync()
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

        if (collegeLicenceInformationList != null)
        {
            var userIds = await this.context.Credentials
                .Where(credential => credential.PartyId == notification.PartyId)
                .Select(credential => credential.UserId)
                .ToListAsync(cancellationToken);

            foreach (var userId in userIds)
            {
                await this.keycloakClient.UpdateUser(userId, (user) => user.SetCollegeLicenceInformation(collegeLicenceInformationList));
            }
        }
    }
}
