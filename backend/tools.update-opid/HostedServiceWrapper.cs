namespace UpdateOpId;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

internal sealed class HostedServiceWrapper : IHostedService
{
    private readonly IUpdateOpIdService service;
    private readonly IHostApplicationLifetime appLifetime;
    private readonly ILogger logger;
    private int? exitCode;

    public HostedServiceWrapper(
        IUpdateOpIdService service,
        IHostApplicationLifetime appLifetime,
        ILogger<HostedServiceWrapper> logger)
    {
        this.service = service;
        this.appLifetime = appLifetime;
        this.logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        this.appLifetime.ApplicationStarted.Register(async () =>
        {
            try
            {
                await this.service.UpdateOpIdAsync();
                this.exitCode = 0;
            }
            catch (Exception ex)
            {
                this.logger.LogUnhandledException(ex);
                this.exitCode = 1;
            }
            finally
            {
                this.appLifetime.StopApplication();
            }
        });

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Environment.ExitCode = this.exitCode.GetValueOrDefault(-1);
        return Task.CompletedTask;
    }
}

public static partial class HostedServiceWrapperLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Error, "Unhandled exception in HostedServiceWrapper.")]
    public static partial void LogUnhandledException(this ILogger logger, Exception e);
}
