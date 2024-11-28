namespace DoWork;

using Microsoft.EntityFrameworkCore;

using Pidp.Data;
using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Infrastructure.HttpClients.Plr;

public class DoWorkService(IKeycloakAdministrationClient keycloakClient, PidpDbContext context, PlrClient plrClient) : IDoWorkService
{
    private readonly IKeycloakAdministrationClient keycloakClient = keycloakClient;
    private readonly PidpDbContext context = context;

    private readonly PlrClient plrClient = plrClient;

    public async Task DoWorkAsync()
    {
        var parties = await this.context.Parties
            .Include(party => party.Credentials)
            .ToListAsync();

        foreach (var party in parties)
        {
            var standingsDigest = await this.plrClient.GetStandingsDigestAsync(party.Cpn);
            if (standingsDigest.With(IdentifierType.Pharmacist).HasGoodStanding)
            {
                foreach (var userId in party.Credentials.Select(credential => credential.UserId))
                {
                    await this.keycloakClient.UpdateUser(userId, user => user.SetIsPharm(standingsDigest.With(IdentifierType.Pharmacist).HasGoodStanding));
                }
            }
        }
    }
}
