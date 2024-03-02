namespace Pidp.Features.CommonHandlers;

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

public class AssignAttributesInKeycloakAfterPlrCpnLookupFound : INotificationHandler<PlrCpnLookupFound>
{
    private readonly IKeycloakAdministrationClient client;

    public AssignAttributesInKeycloakAfterPlrCpnLookupFound(IKeycloakAdministrationClient client) => this.client = client;

    public async Task Handle(PlrCpnLookupFound notification, CancellationToken cancellationToken)
    {
        // TODO: what to do if this fails?
        foreach (var userId in notification.UserIds)
        {
            await this.client.UpdateUser(userId, (user) => user.SetCpn(notification.Cpn));
        }
    }
}

public class AssignKeycloakRolesAfterPlrCpnLookupFound : INotificationHandler<PlrCpnLookupFound>
{
    private readonly IClock clock;
    private readonly IKeycloakAdministrationClient keycloakClient;
    private readonly PidpDbContext context;

    public AssignKeycloakRolesAfterPlrCpnLookupFound(
        IClock clock,
        IKeycloakAdministrationClient keycloakClient,
        PidpDbContext context)
    {
        this.clock = clock;
        this.keycloakClient = keycloakClient;
        this.context = context;
    }

    public async Task Handle(PlrCpnLookupFound notification, CancellationToken cancellationToken)
    {
        // TODO: what to do if any of this fails?
        if (!notification.StandingsDigest.HasGoodStanding)
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
    private readonly PidpDbContext context;
    private readonly string clientId;

    public UpdateBCProviderAfterPlrCpnLookupFound(
        IBCProviderClient bcProviderClient,
        PidpDbContext context,
        PidpConfiguration config)
    {
        this.bcProviderClient = bcProviderClient;
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

        var attributes = new BCProviderAttributes(this.clientId)
            .SetCpn(notification.Cpn)
            .SetIsRnp(notification.StandingsDigest.With(ProviderRoleType.RegisteredNursePractitioner).HasGoodStanding)
            .SetIsMd(notification.StandingsDigest.With(ProviderRoleType.MedicalDoctor).HasGoodStanding);

        // If the user becomes regulated, they lose their MOA status
        if (notification.StandingsDigest.HasGoodStanding)
        {
            attributes.SetIsMoa(false);
        }

        await this.bcProviderClient.UpdateAttributes(userPrincipalName, attributes.AsAdditionalData());
    }
}
