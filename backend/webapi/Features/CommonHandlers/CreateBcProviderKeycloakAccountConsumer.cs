namespace Pidp.Features.CommonHandlers;

using Flurl;
using MassTransit;

using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Data;
using Microsoft.EntityFrameworkCore;
using Pidp.Infrastructure.HttpClients.BCProvider;
using Pidp.Models.DomainEvents;

public class CreateBcProviderKeycloakAccountConsumer : IConsumer<BCProviderCreated>
{
    private readonly IBCProviderClient bcProviderClient;
    private readonly IKeycloakAdministrationClient client;
    private readonly PidpDbContext context;
    private readonly ILogger<CreateBcProviderKeycloakAccountConsumer> logger;
    private readonly Url keycloakAdministrationUrl;

    public CreateBcProviderKeycloakAccountConsumer(
        IBCProviderClient bcProviderClient,
        IKeycloakAdministrationClient client,
        PidpDbContext context,
        ILogger<CreateBcProviderKeycloakAccountConsumer> logger,
        PidpConfiguration config)
    {
        this.bcProviderClient = bcProviderClient;
        this.client = client;
        this.context = context;
        this.logger = logger;
        this.keycloakAdministrationUrl = Url.Parse(config.Keycloak.AdministrationUrl);
    }

    public async Task Consume(ConsumeContext<BCProviderCreated> context)
    {
        var message = context.Message;
        var userId = await this.CreateKeycloakUser(message.FirstName, message.LastName, message.UserPrincipalName);
        if (userId == null)
        {
            this.logger.LogCreateBcProviderKeycloakAccountFailed(message.UserPrincipalName);
            await this.bcProviderClient.DeleteBCProviderAccount(message.UserPrincipalName);
            throw new InvalidOperationException("Error when creating BCProvider User in Keycloak");
        }
        await this.client.UpdateUser(userId.Value, user => user.SetOpId(message.OpId!));

        var credential = await this.context.Credentials
            .Where(credential => credential.PartyId == message.PartyId && credential.IdpId == message.UserPrincipalName)
            .SingleAsync();

        await this.context.SaveChangesAsync();
    }

    private async Task<Guid?> CreateKeycloakUser(string firstName, string lastName, string userPrincipalName)
    {
        var newUser = new UserRepresentation
        {
            Enabled = true,
            FirstName = firstName,
            LastName = lastName,
            Username = this.GenerateMohKeycloakUsername(userPrincipalName)
        };
        return await this.client.CreateUser(newUser);
    }

    /// <summary>
    /// The expected Ministry of Health Keycloak username for this user. The schema is {IdentityProviderIdentifier}@{IdentityProvider}.
    /// Most of our Credentials come to us from Keycloak and so the username is saved as-is in the column IdpId.
    /// However, we create BC Provider Credentials directly; so the User Principal Name is saved to the database without the suffix.
    /// There are also two inconsistencies with how the MOH Keycloak handles BCP usernames:
    /// 1. The username suffix is @bcp rather than @bcprovider_aad.
    /// 2. This suffix is only added in Test and Production; there is no suffix at all for BCP Users in the Dev Keycloak.
    /// </summary>
    private string GenerateMohKeycloakUsername(string userPrincipalName)
    {
        if (this.keycloakAdministrationUrl.Host == "user-management-dev.api.hlth.gov.bc.ca")
        {
            return userPrincipalName;
        }

        return userPrincipalName + "@bcp";
    }

}

public static partial class CreateBcProviderKeycloakAccountConsumerLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Error, "Error when creating a Keycloak account for BCProvider user {upn}.")]
    public static partial void LogCreateBcProviderKeycloakAccountFailed(this ILogger<CreateBcProviderKeycloakAccountConsumer> logger, string upn);
}
