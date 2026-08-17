namespace Pidp.Models.Lookups;

public interface ILookupDataGenerator<T>
{
    public IEnumerable<T> Generate();
}
