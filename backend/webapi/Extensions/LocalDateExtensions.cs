namespace Pidp.Extensions;

using NodaTime;
using System.Globalization;

public static class LocalDateExtensions
{
    /// <summary>
    /// Formats the LocalDate as an ISO date string, i.e. "yyyy-MM-dd".
    /// </summary>
    /// <param name="date"></param>
    public static string ToIsoDateString(this LocalDate date) => date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
}
