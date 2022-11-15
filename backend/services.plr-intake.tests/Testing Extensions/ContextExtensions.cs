namespace PlrIntakeTests.TestingExtensions;

using PlrIntake.Data;

public static class ContextExtensions
{
    public static T Has<T>(this PlrDbContext context, T thing)
    {
        context.Add(thing!);
        context.SaveChanges();
        return thing;
    }

    public static IEnumerable<T> HasSome<T>(this PlrDbContext context, IEnumerable<T> things)
    {
        context.AddRange(things.Cast<object>());
        context.SaveChanges();
        return things;
    }
}
