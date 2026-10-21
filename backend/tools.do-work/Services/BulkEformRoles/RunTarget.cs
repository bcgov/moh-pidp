namespace DoWork.Services.BulkEformRoles;

using System.Data.Common;

using Pidp;

/// <summary>
/// Prints what an apply run is about to change, and makes the operator confirm it.
///
/// The database and Keycloak are configured independently, and the checked-in appsettings.json pairs a
/// localhost database with a dev Keycloak realm - so overriding one and forgetting the other is an easy
/// mistake to make and an invisible one to detect. Keycloak answers a role removal for a User it does not
/// have with a 404 rather than a 204, so a cross-environment run does not silently delete anyone; it
/// reports RevokeFailed for every Party and stages an Error-severity Business Event for each. Loud, but
/// only after the fact. Reading the two targets side by side beforehand is what actually prevents it.
/// </summary>
public static class RunTarget
{
    public static void Describe(PidpConfiguration config)
    {
        Console.WriteLine("Target:");
        Console.WriteLine($"  Database  {DescribeDatabase(config.ConnectionStrings.PidpDatabase)}");
        Console.WriteLine($"  Keycloak  {config.Keycloak.AdministrationUrl} (client {config.Keycloak.AdministrationClientId})");
        Console.WriteLine($"  Realm     {config.Keycloak.RealmUrl}");
    }

    /// <param name="phrase">What the operator must type. The name of the thing being changed, so typing it is a reading of the target.</param>
    public static bool Confirm(string phrase)
    {
        Console.WriteLine();
        Console.Write($"This will change live access. Type '{phrase}' to proceed, or anything else to abort: ");

        var typed = Console.ReadLine()?.Trim();

        if (string.Equals(typed, phrase, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        Console.WriteLine("Aborted. Nothing was changed.");

        return false;
    }

    /// <summary>
    /// Host, port and database only. The connection string carries a password, which must not reach the
    /// console or a captured log.
    /// </summary>
    /// <param name="connectionString"></param>
    private static string DescribeDatabase(string? connectionString)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return "(not configured)";
        }

        try
        {
            var builder = new DbConnectionStringBuilder { ConnectionString = connectionString };

            return $"{Value(builder, "Host")}:{Value(builder, "Port")}/{Value(builder, "Database")} as {Value(builder, "Username")}";
        }
        catch (ArgumentException)
        {
            return "(unparseable connection string)";
        }
    }

    private static string Value(DbConnectionStringBuilder builder, string key)
        => builder.TryGetValue(key, out var value) ? Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture) ?? "?" : "?";
}
