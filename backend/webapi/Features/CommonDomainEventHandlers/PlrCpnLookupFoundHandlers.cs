namespace Pidp.Features.CommonDomainEventHandlers;

using MediatR;
using Microsoft.EntityFrameworkCore;
using NodaTime;

using Pidp.Data;
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
        var userPrincipalName = await this.context.Credentials
            .Where(credential => credential.PartyId == notification.PartyId
                && credential.IdentityProvider == IdentityProviders.BCProvider)
            .Select(credential => credential.IdpId)
            .SingleOrDefaultAsync(cancellationToken);

        if (userPrincipalName == null)
        {
            return;
        }

        var plrStanding = await this.plrClient.GetStandingsDigestAsync(notification.Cpn);
        var attributes = new BCProviderAttributes(this.clientId)
            .SetCpn(notification.Cpn)
            .SetIsRnp(plrStanding.With(ProviderRoleType.RegisteredNursePractitioner).HasGoodStanding)
            .SetIsMd(plrStanding.With(ProviderRoleType.MedicalDoctor).HasGoodStanding);

        // If the user becomes regulated, they lose their MOA status
        if (plrStanding.HasGoodStanding)
        {
            attributes.SetIsMoa(false);
        }

        await this.bcProviderClient.UpdateAttributes(userPrincipalName, attributes.AsAdditionalData());
    }
}
