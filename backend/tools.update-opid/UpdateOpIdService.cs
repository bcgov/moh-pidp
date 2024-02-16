namespace UpdateOpId;

using Microsoft.EntityFrameworkCore;
using NanoidDotNet;

using Pidp.Data;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.Keycloak;

public class UpdateOpIdService : IUpdateOpIdService
{
    private readonly PidpDbContext context;
    private readonly IKeycloakAdministrationClient keycloakClient;

    public UpdateOpIdService(PidpDbContext context, IKeycloakAdministrationClient keycloakClient)
    {
        this.context = context;
        this.keycloakClient = keycloakClient;
    }

    public async Task UpdateOpIdAsync()
    {
        var partyIds = await this.context.Credentials
            .Where(credential => credential.IdentityProvider == IdentityProviders.BCServicesCard)
            .Select(credential => credential.PartyId)
            .ToListAsync();

        var parties = await this.context.Parties
            .Where(party => partyIds.Any(partyId => partyId == party.Id))
            .ToListAsync();

        foreach (var party in parties)
        {
            if (party.OpId != null)
            {
                continue;
            }
            party.OpId = Nanoid.Generate("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ");

            var userIds = await this.context.Credentials
                .Where(credential => credential.PartyId == party.Id)
                .Select(credential => credential.UserId)
                .ToListAsync();

            foreach (var userId in userIds)
            {
                await this.keycloakClient.UpdateUserOpId(userId, party.OpId);
            }
        }

        await this.context.SaveChangesAsync();
    }
}
