namespace Pidp.Infrastructure.HealthChecks;

using global::HealthChecks.NpgSql;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;


public class LoggingNpgSqlHealthCheck : IHealthCheck
{
    private readonly NpgSqlHealthCheck _npgsqlHealthCheck;
    private readonly ILogger<LoggingNpgSqlHealthCheck> _logger;

    public LoggingNpgSqlHealthCheck(PidpConfiguration config, ILogger<LoggingNpgSqlHealthCheck> logger)
    {
        _npgsqlHealthCheck = new NpgSqlHealthCheck(config.ConnectionStrings.PidpDatabase, "SELECT 1;");
        _logger = logger;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            return await _npgsqlHealthCheck.CheckHealthAsync(context, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Health check failed");
            return HealthCheckResult.Unhealthy("Database connection failed", ex);
        }
    }
}