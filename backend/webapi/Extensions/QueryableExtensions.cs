namespace Pidp.Extensions;

public static class QueryableExtensions
{
    /// <summary>
    /// Conditionally applys a new condition to the query.
    /// </summary>
    public static IQueryable<T> If<T>(this IQueryable<T> source, bool condition, Func<IQueryable<T>, IQueryable<T>> branch)
        => condition ? branch(source) : source;
}
