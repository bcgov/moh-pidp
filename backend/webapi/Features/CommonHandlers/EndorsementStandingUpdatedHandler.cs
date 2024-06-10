namespace Pidp.Features.CommonHandlers;

using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;

using Pidp.Data;
using Pidp.Extensions;
using static Pidp.Features.CommonHandlers.UpdateBcProviderAttributesConsumer;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.BCProvider;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models.DomainEvents;

public class UpdateBCProviderAfterEndorsementStandingUpdated : INotificationHandler<EndorsementStandingUpdated>
{
    private readonly IBus bus;
    private readonly IPlrClient plrClient;
    private readonly PidpDbContext context;
    private readonly string clientId;

    public UpdateBCProviderAfterEndorsementStandingUpdated(
        IBus bus,
        IPlrClient plrClient,
        PidpDbContext context,
        PidpConfiguration config)
    {
        this.bus = bus;
        this.plrClient = plrClient;
        this.context = context;
        this.clientId = config.BCProviderClient.ClientId;
    }

    public async Task Handle(EndorsementStandingUpdated notification, CancellationToken cancellationToken)
    {
        var party = await this.context.Parties
            .Where(party => party.Id == notification.PartyId)
            .Select(party => new
            {
                party.Cpn,
                Upn = party.Credentials
                    .Where(credential => credential.IdentityProvider == IdentityProviders.BCProvider)
                    .Select(credential => credential.IdpId)
                    .SingleOrDefault()
            })
            .SingleAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(party.Upn))
        {
            return;
        }

        var plrStanding = await this.plrClient.GetStandingsDigestAsync(party.Cpn);
        var endorsingCpns = await this.context.ActiveEndorsingParties(notification.PartyId)
            .Select(party => party.Cpn)
            .ToListAsync(cancellationToken);
        var endorsingPlrDigest = await this.plrClient.GetAggregateStandingsDigestAsync(endorsingCpns);

        var attributes = new BCProviderAttributes(this.clientId)
            .SetIsMoa(!plrStanding.HasGoodStanding && endorsingPlrDigest.HasGoodStanding)
            .SetEndorserData(endorsingPlrDigest
                .WithGoodStanding()
                .With(BCProviderAttributes.EndorserDataEligibleIdentifierTypes)
                .Cpns);

        await this.bus.Publish(new UpdateBcProviderAttributes(party.Upn, attributes.AsAdditionalData()), cancellationToken);
    }
}
