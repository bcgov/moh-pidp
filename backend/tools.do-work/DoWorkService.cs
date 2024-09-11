namespace DoWork;

using Microsoft.EntityFrameworkCore;

using Pidp.Data;
using Pidp.Infrastructure.Auth;
using Pidp.Infrastructure.HttpClients.Keycloak;
using Pidp.Models;
using Pidp.Models.Lookups;

public class DoWorkService(IKeycloakAdministrationClient keycloakClient, PidpDbContext context) : IDoWorkService
{
    private readonly IKeycloakAdministrationClient keycloakClient = keycloakClient;
    private readonly PidpDbContext context = context;

    public async Task DoWorkAsync()
    {
        // await this.PopulateJobs();

        var batch = await this.context.Jobs
            .Where(job => !job.Complete)
            .Take(10)
            .ToListAsync();

        foreach (var job in batch)
        {
            try
            {
                job.Complete = await this.keycloakClient.AssignAccessRoles(job.UserId, MohKeycloakEnrolment.SAEforms);

                if (!job.Complete)
                {
                    Console.WriteLine($"Error assigning access roles to user {job.UserId}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception assigning access roles to user {job.UserId}: {ex.Message}");
            }
        }

        await this.context.SaveChangesAsync();
    }

    private async Task PopulateJobs()
    {
        var jobs = await this.context.Credentials
            .Where(credential => credential.IdentityProvider == IdentityProviders.BCProvider
                && credential.Party!.AccessRequests.Any(request => request.AccessTypeCode == AccessTypeCode.SAEforms))
            .Select(credential => new Job
            {
                UserId = credential.UserId
            })
            .ToListAsync();

        this.context.Jobs.AddRange(jobs);
        await this.context.SaveChangesAsync();
    }
}
