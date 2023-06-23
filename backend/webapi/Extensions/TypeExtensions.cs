namespace Pidp.Extensions;

using System.Reflection;

public static class TypetExtensions
{
    public static IEnumerable<Type> GetDerivedTypes(this Type type)
    {
        return Assembly.GetAssembly(type)!
            .GetTypes()
            .Where(t => t.IsSubclassOf(type));
    }
}
