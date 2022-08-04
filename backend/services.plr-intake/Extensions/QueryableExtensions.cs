namespace PlrIntake.Extensions;

using PlrIntake.Models;

public static class QueryableExtensions
{
    /// <summary>
    /// Filters out Records that have been "deleted" from PLR.
    /// </summary>
    /// <param name="query"></param>
    public static IQueryable<PlrRecord> ExcludeDeleted(this IQueryable<PlrRecord> query) => query.Where(record => record.StatusReasonCode != "REM");

    /// <summary>
    /// Conditionally applys a new condition to the query.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="condition"></param>
    /// <param name="branch"></param>
    public static IQueryable<T> If<T>(this IQueryable<T> source, bool condition, Func<IQueryable<T>, IQueryable<T>> branch) => condition ? branch(source) : source;
}
