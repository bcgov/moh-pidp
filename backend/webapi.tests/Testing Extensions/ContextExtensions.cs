namespace PidpTests.TestingExtensions;

using Pidp.Data;

public static class ContextExtensions
{
    public static T Has<T>(this PidpDbContext context, T thing)
    {
        context.Add(thing!);
        context.SaveChanges();
        return thing;
    }

    public static IEnumerable<T> HasSome<T>(this PidpDbContext context, IEnumerable<T> things)
    {
        context.AddRange(things.Cast<object>());
        context.SaveChanges();
        return things;
    }
}
