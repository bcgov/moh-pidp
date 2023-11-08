
namespace Pidp.Infrastructure.HealthChecks;

public class HealthCheckTag
{
    /// <summary>
    /// Represents a "backgroundServices" check.
    /// </summary>
    public static readonly HealthCheckTag BackgroundServices = new("background-services");

    /// <summary>
    /// Represents a "liveness" check.
    /// A service that fails a liveness check is considered to be unrecoverable and has to be restarted by the orchestrator.
    /// </summary>
    public static readonly HealthCheckTag Liveness = new("liveness");

    /// <summary>
    /// Represents a "readiness" check.
    /// A service that fails a readiness check is considered to be unable to serve traffic temporarily.
    /// </summary>
    public static readonly HealthCheckTag Readiness = new("readiness");

    public string Value { get; }

    private HealthCheckTag(string value) => this.Value = value;

    public static implicit operator string(HealthCheckTag type) => type.Value;
}
