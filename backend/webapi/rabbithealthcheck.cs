using System.Collections.Concurrent;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using RabbitMQ.Client;

namespace HealthChecks.RabbitMQ;

/// <summary>
/// A health check for RabbitMQ services.
/// </summary>
public class RabbitMQHealthCheck2 : IHealthCheck
{
    private static readonly ConcurrentDictionary<RabbitMQHealthCheckOptions, IConnection> _connections = new();

    private IConnection? _connection;
    private readonly RabbitMQHealthCheckOptions _options;

    public RabbitMQHealthCheck2(RabbitMQHealthCheckOptions options)
    {
        _options = options;
        _connection = options.Connection;

        if (_connection is null && _options.ConnectionFactory is null && _options.ConnectionUri is null)
        {
            throw new ArgumentException("A connection, connnection factory, or connection string must be set!", nameof(options));
        }
    }

    /// <inheritdoc />
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        // TODO: cancellationToken unused, see https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks/issues/714
        try
        {
            using var model = EnsureConnection().CreateModel();
            // System.Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(this._connection));
            System.Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(model));
            return HealthCheckResultTask.Healthy;
        }
        catch (Exception ex)
        {
            return Task.FromResult(new HealthCheckResult(context.Registration.FailureStatus, exception: ex));
        }
    }

    private IConnection EnsureConnection()
    {
        _connection ??= _connections.GetOrAdd(_options, _ =>
            {
                var factory = _options.ConnectionFactory;

                if (factory is null)
                {
                    factory = new ConnectionFactory
                    {
                        Uri = _options.ConnectionUri,
                        AutomaticRecoveryEnabled = true
                    };

                    if (_options.RequestedConnectionTimeout is not null)
                    {
                        ((ConnectionFactory)factory).RequestedConnectionTimeout = _options.RequestedConnectionTimeout.Value;
                    }

                    if (_options.Ssl is not null)
                    {
                        ((ConnectionFactory)factory).Ssl = _options.Ssl;
                    }
                }

                return factory.CreateConnection();
            });

        return _connection;
    }
}

internal static class HealthCheckResultTask
{
    /// <summary>
    /// Memory optimization that allows to not allocate new Task on green path in non-async health checks.
    /// </summary>
    public static readonly Task<HealthCheckResult> Healthy = Task.FromResult(HealthCheckResult.Healthy());
}
