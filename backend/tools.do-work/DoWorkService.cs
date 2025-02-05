namespace DoWork;

using Microsoft.EntityFrameworkCore;
using Pidp;
using Pidp.Data;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.BCProvider;
using Pidp.Infrastructure.HttpClients.Plr;

public class PartyDto
{
    public string? Cpn { get; set; }
    public string? Upn { get; set; }
}

public class DoWorkService(
    IBCProviderClient bcProviderClient,
    PidpDbContext context,
    IPlrClient plrClient,
    PidpConfiguration config) : IDoWorkService
{
    private readonly IBCProviderClient bcProviderClient = bcProviderClient;
    private readonly PidpDbContext context = context;
    private readonly IPlrClient plrClient = plrClient;
    private readonly string clientId = config.BCProviderClient.ClientId;


    public async Task DoWorkAsync()
    {
        var parties = await this.context.Parties
            .Include(party => party.Credentials)
            .Where(party => party.Cpn != null
                && party.Credentials.Any(credential => credential.IdentityProvider == IdentityProviders.BCProvider))
            .ToListAsync();
        foreach (var party in parties)
        {
            var upn = party.Credentials
                    .Where(credential => credential.IdentityProvider == IdentityProviders.BCProvider)
                    .Select(credential => credential.IdpId)
                    .FirstOrDefault();
            if (party.Cpn == null || upn == null)
            {
                continue;
            }
            var plrStanding = await this.plrClient.GetStandingsDigestAsync(party.Cpn);

            if (plrStanding.With(IdentifierType.Pharmacist).HasGoodStanding)
            {
                var attributes = new BCProviderAttributes(this.clientId)
                    .SetIsPharm(true);

                await this.bcProviderClient.UpdateAttributes(upn, attributes.AsAdditionalData());
            }
        }
    }
}

