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

        Thread.Sleep(40000); // Wait for 40 seconds to allow the application to fully start before executing the service

        this.logger.LogServiceStarting();

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
                this.logger.LogServiceError(ex);
            }

            // Run once a day (24 hours)
            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }

        this.logger.LogServiceStopping();
    }
}

public static partial class PharmacyStaffDeactivationLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Information, "Pharmacy Staff Deactivation Hosted Service is starting.")]
    public static partial void LogServiceStarting(this ILogger logger);

    [LoggerMessage(2, LogLevel.Error, "Error occurred executing Pharmacy Staff Deactivation Hosted Service.")]
    public static partial void LogServiceError(this ILogger logger, Exception ex);

    [LoggerMessage(3, LogLevel.Information, "Pharmacy Staff Deactivation Hosted Service is stopping.")]
    public static partial void LogServiceStopping(this ILogger logger);
}
