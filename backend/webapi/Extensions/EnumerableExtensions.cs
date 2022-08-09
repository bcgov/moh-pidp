namespace Pidp.Extensions;

public static class EnumerableExtensions
{
    /// <summary>
    /// Conditionally applys a new condition to the enumerable.
    /// </summary>
    public static IEnumerable<T> If<T>(this IEnumerable<T> source, bool condition, Func<IEnumerable<T>, IEnumerable<T>> branch)
        => condition ? branch(source) : source;
}
