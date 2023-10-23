namespace Pidp.Features.CommonDomainEventHandlers;

using MediatR;
using Microsoft.EntityFrameworkCore;
using NodaTime;

using Pidp.Data;
using Pidp.Extensions;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.BCProvider;
using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Infrastructure.HttpClients.Plr;
using Pidp.Models;
using Pidp.Models.DomainEvents;

public class AssignCpnInKeycloakAfterPlrCpnLookupFound : INotificationHandler<PlrCpnLookupFound>
{
    private readonly IKeycloakAdministrationClient client;

    public AssignCpnInKeycloakAfterPlrCpnLookupFound(IKeycloakAdministrationClient client) => this.client = client;

    public async Task Handle(PlrCpnLookupFound notification, CancellationToken cancellationToken)
    {
        // TODO: what to do if this fails?
        foreach (var userId in notification.UserIds)
        {
            await this.client.UpdateUserCpn(userId, notification.Cpn);
        }
    }
}

public class AssignKeycloakRolesAfterPlrCpnLookupFound : INotificationHandler<PlrCpnLookupFound>
{
    private readonly IClock clock;
    private readonly IKeycloakAdministrationClient keycloakClient;
    private readonly IPlrClient plrClient;
    private readonly PidpDbContext context;

    public AssignKeycloakRolesAfterPlrCpnLookupFound(
        IClock clock,
        IKeycloakAdministrationClient keycloakClient,
        IPlrClient plrClient,
        PidpDbContext context)
    {
        this.clock = clock;
        this.keycloakClient = keycloakClient;
        this.plrClient = plrClient;
        this.context = context;
    }

    public async Task Handle(PlrCpnLookupFound notification, CancellationToken cancellationToken)
    {
        // TODO: what to do if any of this fails?
        if (!await this.plrClient.GetStandingAsync(notification.Cpn))
        {
            return;
        }

        foreach (var userId in notification.UserIds)
        {
            if (await this.keycloakClient.AssignAccessRoles(userId, MohKeycloakEnrolment.PractitionerLicenceStatus))
            {
                this.context.BusinessEvents.Add(LicenceStatusRoleAssigned.Create(notification.PartyId, MohKeycloakEnrolment.PractitionerLicenceStatus, this.clock.GetCurrentInstant()));
            }
        }
    }
}

public class UpdateBCProviderAfterPlrCpnLookupFound : INotificationHandler<PlrCpnLookupFound>
{
    private readonly IBCProviderClient bcProviderClient;
    private readonly IPlrClient plrClient;
    private readonly PidpDbContext context;
    private readonly string clientId;

    public UpdateBCProviderAfterPlrCpnLookupFound(
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

    public async Task Handle(PlrCpnLookupFound notification, CancellationToken cancellationToken)
    {
        var plrStanding = await this.plrClient.GetStandingsDigestAsync(notification.Cpn);

        if (plrStanding
            .With(IdentifierType.PhysiciansAndSurgeons, IdentifierType.Nurse, IdentifierType.Midwife)
            .HasGoodStanding)
        {
            await this.UpdateEndorserData(notification);
        }

        var userPrincipalName = await this.context.Credentials
            .Where(credential => credential.PartyId == notification.PartyId
                && credential.IdentityProvider == IdentityProviders.BCProvider)
            .Select(credential => credential.IdpId)
            .SingleOrDefaultAsync(cancellationToken);

        if (userPrincipalName == null)
        {
            return;
        }

        // If the user becomes regulated, they lose their MOA status
        if (plrStanding.HasGoodStanding)
        {
            var attribute = new BCProviderAttributes(this.clientId)
                .SetIsMoa(false);

            await this.bcProviderClient.UpdateAttributes(userPrincipalName, attribute.AsAdditionalData());
        }


        var attributes = new BCProviderAttributes(this.clientId)
            .SetCpn(notification.Cpn)
            .SetIsRnp(plrStanding.With(ProviderRoleType.RegisteredNursePractitioner).HasGoodStanding)
            .SetIsMd(plrStanding.With(ProviderRoleType.MedicalDoctor).HasGoodStanding);

        await this.bcProviderClient.UpdateAttributes(userPrincipalName, attributes.AsAdditionalData());
    }

    private async Task UpdateEndorserData(PlrCpnLookupFound notification)
    {
        var endorsementRelations = await this.context.ActiveEndorsementRelationships(notification.PartyId)
            .Select(relationship => new
            {
                relationship.PartyId,
                relationship.Party!.Cpn,
                UserPrincipalName = relationship.Party.Credentials
                    .Where(credential => credential.IdentityProvider == IdentityProviders.BCProvider)
                    .Select(credential => credential.IdpId)
                    .SingleOrDefault(),
            })
            .ToListAsync();

        foreach (var relation in endorsementRelations)
        {
            if (string.IsNullOrWhiteSpace(relation.UserPrincipalName)
                || await this.plrClient.GetStandingAsync(relation.Cpn))
            {
                // User either has no BC Provider or has a licence in good standing and so can't be an MOA.
                continue;
            }

            var endorsingCpns = await this.context.ActiveEndorsementRelationships(relation.PartyId)
                .Select(relationship => relationship.Party!.Cpn)
                .ToListAsync();

            var endorsingPartiesStanding = await this.plrClient.GetAggregateStandingsDigestAsync(endorsingCpns);

            var relationBCProviderAttributes = new BCProviderAttributes(this.clientId)
                .SetIsMoa(true)
                .SetEndorserData(endorsingPartiesStanding
                    .WithGoodStanding()
                    .With(IdentifierType.PhysiciansAndSurgeons, IdentifierType.Nurse, IdentifierType.Midwife)
                    .Cpns);
            await this.bcProviderClient.UpdateAttributes(relation.UserPrincipalName, relationBCProviderAttributes.AsAdditionalData());
        }
    }
}
