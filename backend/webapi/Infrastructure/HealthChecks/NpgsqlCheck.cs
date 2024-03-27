namespace Pidp.Infrastructure.HealthChecks;

using global::HealthChecks.NpgSql;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using Npgsql;

public class LoggingNpgSqlHealthCheck : IHealthCheck
{
    private readonly NpgSqlHealthCheck _npgsqlHealthCheck;
    private readonly ILogger<LoggingNpgSqlHealthCheck> _logger;

    private readonly PidpConfiguration _config;
    

    public LoggingNpgSqlHealthCheck(PidpConfiguration config, ILogger<LoggingNpgSqlHealthCheck> logger)
    {
        _config = config;
        _logger = logger;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            using (var connection = new NpgsqlConnection(_config.ConnectionStrings.PidpDatabase))
            {
                //_connectionAction?.Invoke(connection);

                await connection.OpenAsync(cancellationToken);
                _logger.LogCritical("Connection opened.");

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT 1;";
                    await command.ExecuteScalarAsync(cancellationToken);
                }
                _logger.LogCritical("Command executed.");

                return HealthCheckResult.Healthy();
            }
        }
        catch (Exception ex)
        {
            return new HealthCheckResult(context.Registration.FailureStatus, exception: ex);
        }
    }
}