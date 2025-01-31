namespace DoWork;

using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.Drives.Item.Items.Item.Workbook.Functions.Vdb;
using Pidp;
using Pidp.Data;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.BCProvider;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models.Lookups;

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
        var cpns = await this.context.Parties
            .Where(party => party.Cpn != null
                && party.Credentials.Any(credential => credential.IdentityProvider == IdentityProviders.BCProvider)
                && party.LicenceDeclaration.CollegeCode == CollegeCode.Pharmacists)
                .Select(party => party.Cpn)
            .ToListAsync();
        Console.WriteLine(">>>>CPNs loaded!", + cpns.Count);
        // var parties = await this.context.Parties
        //     .Include(party => party.Credentials)
        //     .Where(party => party.Cpn != null
        //         && party.Credentials.Any(credential => credential.IdentityProvider == IdentityProviders.BCProvider))
        //     .ToListAsync();
        // Console.WriteLine(">>>>Parties loaded!");
        // Console.WriteLine(">>>>Parties count: " + parties.Count);
        // Console.WriteLine(">>>>Updating BCProvider attributes...");

        // foreach (var party in parties)
        // {
        //     var userPrincipalName = party.Credentials
        //         .Where(credential => credential.IdentityProvider == IdentityProviders.BCProvider)
        //         .Select(credential => credential.IdpId)
        //         .FirstOrDefault();

        //     if (userPrincipalName == null)
        //     {
        //         return;
        //     }

        //     var plrStanding = await this.plrClient.GetStandingsDigestAsync(party.Cpn);

        //     var attributes = new BCProviderAttributes(this.clientId)
        //         .SetIsPharm(plrStanding.With(IdentifierType.Pharmacist).HasGoodStanding);

        //     await this.bcProviderClient.UpdateAttributes(userPrincipalName, attributes.AsAdditionalData());
        // }
    }
}
