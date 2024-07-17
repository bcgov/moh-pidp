namespace PidpTests.TestingExtensions;

using Pidp.Data;
using Pidp.Models;

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

    public static Party HasAParty(this PidpDbContext context, Action<Party>? config = null)
    {
        var party = new Party();
        config?.Invoke(party);
        if (party.Credentials.Count == 0)
        {
            party.Credentials.Add(new Credential { UserId = Guid.NewGuid() });
        }

        return context.Has(party);
    }
}
