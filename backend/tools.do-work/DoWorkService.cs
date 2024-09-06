namespace DoWork;

using Microsoft.EntityFrameworkCore;

using Pidp.Data;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Models.Lookups;

public class DoWorkService(IKeycloakAdministrationClient keycloakClient, PidpDbContext context) : IDoWorkService
{
    private readonly IKeycloakAdministrationClient keycloakClient = keycloakClient;
    private readonly PidpDbContext context = context;

    public async Task DoWorkAsync()
    {
        var credentials = await this.context.Parties
            .Where(party => party.AccessRequests.Any(request => request.AccessTypeCode == AccessTypeCode.SAEforms))
            .SelectMany(party => party.Credentials)
            .Where(credential => credential.IdentityProvider == IdentityProviders.BCProvider)
            .ToListAsync();

        var counter = 0;
        foreach (var credential in credentials)
        {
            try
            {
                await this.keycloakClient.AssignAccessRoles(credential.UserId, MohKeycloakEnrolment.SAEforms);
                Console.WriteLine($"Successfully assigned access roles to user {credential.UserId}, PartyId {credential.PartyId}");
                counter++;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error assigning access roles to user {credential.UserId}, PartyId {credential.Party?.Id}: {ex.Message}");
            }
        }
        Console.WriteLine($"Assigned roles to {counter} users.");
    }
}
