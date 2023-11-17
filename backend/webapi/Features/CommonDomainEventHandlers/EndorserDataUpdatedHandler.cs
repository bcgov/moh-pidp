namespace Pidp.Features.CommonDomainEventHandlers;

using MediatR;
using Microsoft.EntityFrameworkCore;

using Pidp.Data;
using Pidp.Extensions;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.BCProvider;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models.DomainEvents;

public class UpdateBCProviderAfterEndorserDataUpdated : INotificationHandler<EndorserDataUpdated>
{
    private readonly IBCProviderClient bcProviderClient;
    private readonly IPlrClient plrClient;
    private readonly PidpDbContext context;
    private readonly string clientId;

    public UpdateBCProviderAfterEndorserDataUpdated(
        IBCProviderClient bcProviderClient,
        IPlrClient plrClient,
        PidpDbContext context,
        PidpConfiguration config)
    {
        this.bcProviderClient = bcProviderClient;
        this.plrClient = plrClient;
        this.context = context;
        this.clientId = config.BCProviderClient.ClientId;
    }

    public async Task Handle(EndorserDataUpdated notification, CancellationToken cancellationToken)
    {
        var upn = await this.context.Credentials
            .Where(credential => credential.PartyId == notification.PartyId
                && credential.IdentityProvider == IdentityProviders.BCProvider)
            .Select(credential => credential.IdpId)
            .SingleOrDefaultAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(upn))
        {
            return;
        }

        var endorsingCpns = await this.context.ActiveEndorsingParties(notification.PartyId)
            .Select(party => party.Cpn)
            .ToListAsync(cancellationToken);
        var endorsementPlrDigest = await this.plrClient.GetAggregateStandingsDigestAsync(endorsingCpns);
        var filteredCpns = endorsementPlrDigest
            .WithGoodStanding()
            .With(BCProviderAttributes.EndorserDataEligibleIdentifierTypes)
            .Cpns;

        await this.bcProviderClient.UpdateAttributes(upn, new BCProviderAttributes(this.clientId).SetEndorserData(filteredCpns).AsAdditionalData());
    }
}
