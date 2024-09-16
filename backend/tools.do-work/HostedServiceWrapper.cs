namespace DoWork;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

internal sealed class HostedServiceWrapper(
    IDoWorkService service,
    IHostApplicationLifetime appLifetime,
    ILogger<HostedServiceWrapper> logger) : IHostedService
{
    private readonly IDoWorkService service = service;
    private readonly IHostApplicationLifetime appLifetime = appLifetime;
    private readonly ILogger logger = logger;
    private int? exitCode;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        this.appLifetime.ApplicationStarted.Register(async () =>
        {
            var sw = Stopwatch.StartNew();
            try
            {
                Console.WriteLine(">>>> Starting Work.");
                await this.service.DoWorkAsync();
                this.exitCode = 0;
            }
            catch (Exception ex)
            {
                this.logger.LogUnhandledException(ex);
                this.exitCode = 1;
            }
            finally
            {
                sw.Stop();
                Console.WriteLine($">>>> Work Stopped. Total Run Time: {(sw.ElapsedMilliseconds > 1000 ? sw.ElapsedMilliseconds / 1000 : sw.ElapsedMilliseconds + "m")}s.");
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
