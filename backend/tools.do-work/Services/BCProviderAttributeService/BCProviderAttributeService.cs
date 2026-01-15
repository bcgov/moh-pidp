namespace DoWork.Services.BCProviderAttributeService;

using Microsoft.EntityFrameworkCore;
using Pidp;
using Pidp.Data;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.BCProvider;

public class PartyDto
{
    public string? Upn { get; set; }
    public string? OpId { get; set; }
}

public class BCProviderAttributeService(
    IBCProviderClient bcProviderClient,
    PidpDbContext context,
    PidpConfiguration config) : IBCProviderAttributeService
{
    private readonly IBCProviderClient bcProviderClient = bcProviderClient;
    private readonly PidpDbContext context = context;
    private readonly string clientId = config.BCProviderClient.ClientId;


    public async Task UpdateBCProviderAttributesAsync()
    {
        // Still needs finishing touches and testing
        var parties = await this.context.Parties
            .Where(party => party.Credentials.Any(credential => credential.IdentityProvider == IdentityProviders.BCProvider))
            .Select(party => new PartyDto
            {
                Upn = party.Credentials
                    .Where(credential => credential.IdentityProvider == IdentityProviders.BCProvider)
                    .Select(credential => credential.IdpId)
                    .FirstOrDefault(),
                OpId = party.OpId
            })
            .ToListAsync();
        foreach (var party in parties)
        {
            if (party.OpId == null || party.Upn == null)
            {
                continue;
            }

            {
                var attributes = new BCProviderAttributes(this.clientId)
                    .SetOpId(party.OpId);

                await this.bcProviderClient.UpdateAttributes(party.Upn, attributes.AsAdditionalData());
            }
        }
    }
}
