// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.Diagnostics.HealthChecks;

internal sealed class DbContextHealthCheck2<TContext> : IHealthCheck where TContext : DbContext
{
    private static readonly Func<TContext, CancellationToken, Task<bool>> DefaultTestQuery = (dbContext, cancellationToken) =>
    {
        return dbContext.Database.CanConnectAsync(cancellationToken);
    };

    private readonly ILogger logger;
    private readonly TContext _dbContext;
    private readonly IOptionsMonitor<DbContextHealthCheckOptions<TContext>> _options;

    public DbContextHealthCheck2(ILogger<DbContextHealthCheck2<TContext>> logger, TContext dbContext, IOptionsMonitor<DbContextHealthCheckOptions<TContext>> options)
    {
        ArgumentNullException.ThrowIfNull(dbContext);
        ArgumentNullException.ThrowIfNull(options);

        this.logger = logger;
        _dbContext = dbContext;
        _options = options;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();
        ArgumentNullException.ThrowIfNull(context);

        var options = _options.Get(context.Registration.Name);
        var testQuery = options.CustomTestQuery ?? DefaultTestQuery;

        try
        {
            if (await testQuery(_dbContext, cancellationToken))
            {
                return HealthCheckResult.Healthy();
            }

            this.logger.LogFailBranch("try");
            return new HealthCheckResult(context.Registration.FailureStatus);
        }
        catch (Exception exception)
        {
            this.logger.LogFailBranch("exception");
            return HealthCheckResult.Unhealthy(exception.Message, exception);
        }
        finally
        {
            sw.Stop();
            this.logger.LogElapsedMs(sw.ElapsedMilliseconds);
        }
    }
}

public sealed class DbContextHealthCheckOptions<TContext> where TContext : DbContext
{
    public Func<TContext, CancellationToken, Task<bool>>? CustomTestQuery { get; set; }
}

public static partial class DbHealthLoggingExtensions
{
    [LoggerMessage(1, LogLevel.Warning, "DB healthcheck took {elapsedMs}ms.")]
    public static partial void LogElapsedMs(this ILogger logger, long elapsedMs);

    [LoggerMessage(2, LogLevel.Warning, "DB healthcheck failed in branch {branch}.")]
    public static partial void LogFailBranch(this ILogger logger, string branch);
}
