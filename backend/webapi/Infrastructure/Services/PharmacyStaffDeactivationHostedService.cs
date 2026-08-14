namespace Pidp.Infrastructure.Services;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

public class PharmacyStaffDeactivationHostedService(
    IServiceScopeFactory scopeFactory,
    ILogger<PharmacyStaffDeactivationHostedService> logger) : BackgroundService
{
    private readonly IServiceScopeFactory scopeFactory = scopeFactory;
    private readonly ILogger<PharmacyStaffDeactivationHostedService> logger = logger;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        this.logger.LogInformation("Pharmacy Staff Deactivation Hosted Service is starting.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = this.scopeFactory.CreateScope();
                var service = scope.ServiceProvider.GetRequiredService<IPharmacyStaffDeactivationService>();
                await service.DeactivateExpiredStaffAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error occurred executing Pharmacy Staff Deactivation Hosted Service.");
            }

            // Run once a day (24 hours)
            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }

        this.logger.LogInformation("Pharmacy Staff Deactivation Hosted Service is stopping.");
    }
}
