namespace MigrateCollegeLicenceJson;

using Microsoft.EntityFrameworkCore;
using Pidp.Data;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Infrastructure.HttpClients.Plr;

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
        Console.WriteLine(">>>>Start!");
        var parties = await this.context.Parties
            .Include(party => party.Credentials)
            .Take(1000)
            .ToListAsync();

        foreach (var party in parties)
        {
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
}
