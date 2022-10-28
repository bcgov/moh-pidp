namespace PidpTests;

using Xunit;
using Xunit.Sdk;

public class AssertThatTests
{
    [Theory]
    [MemberData(nameof(SimpleEquivalentCollections))]
    public void CollectionsAreEquivalent_Simple_True(IEnumerable<int> collection1, IEnumerable<int> collection2)
    {
        var predicate = (int a, int b) => a == b;

        AssertThat.CollectionsAreEquivalent(collection1, collection2, predicate);
    }

    public static IEnumerable<object[]> SimpleEquivalentCollections()
    {
        yield return new object[] { Enumerable.Empty<int>(), Enumerable.Empty<int>() };
        yield return new object[] { new[] { 1, 2 }, new[] { 2, 1 } };
        yield return new object[] { new[] { 1, 2, 1 }, new[] { 1, 1, 2 } };
    }

    [Theory]
    [MemberData(nameof(SimpleNotEquivalentCollections))]
    public void CollectionsAreEquivalent_SimpleNotEquivalent_False(IEnumerable<int> collection1, IEnumerable<int> collection2)
    {
        var predicate = (int a, int b) => a == b;

        try
        {
            AssertThat.CollectionsAreEquivalent(collection1, collection2, predicate);

            Assert.True(false, "Assertions passed when they should have failed");
        }
        catch (Exception e) when (e is EqualException or NotEqualException or EmptyException) { }
    }

    public static IEnumerable<object[]> SimpleNotEquivalentCollections()
    {
        yield return new object[] { Enumerable.Empty<int>(), new[] { 1 } };
        yield return new object[] { new[] { 2 }, new[] { 1 } };
        yield return new object[] { new[] { 1, 1, 2 }, new[] { 1, 2, 2 } };
    }

    [Theory]
    [MemberData(nameof(ObjectEquivalentCollections))]
    public void CollectionsAreEquivalent_ObjectEquivalent_True(IEnumerable<ExampleClass1> collection1, IEnumerable<ExampleClass2> collection2)
    {
        var predicate = (ExampleClass1 a, ExampleClass2 b) => a.Match == b.Match;

        AssertThat.CollectionsAreEquivalent(collection1, collection2, predicate);
    }

    public static IEnumerable<object[]> ObjectEquivalentCollections()
    {
        yield return new object[] { Enumerable.Empty<ExampleClass1>(), Enumerable.Empty<ExampleClass2>() };
        yield return new object[] { new[] { new ExampleClass1() }, new[] { new ExampleClass2() } };
        yield return new object[]
        {
            new[] { new ExampleClass1{ Match = "match", SomethingElse = "somethingelse" } },
            new[] { new ExampleClass2{ Match = "match", OtherProp = "otherprop" } }
        };
        yield return new object[]
        {
            new[]
            {
                new ExampleClass1{ Match = "match1", SomethingElse = "somethingelse1" },
                new ExampleClass1{ Match = "match1", SomethingElse = "somethingelse2" },
                new ExampleClass1{ Match = "match2", SomethingElse = "somethingelse3" }
            },
            new[]
            {
                new ExampleClass2{ Match = "match2", OtherProp = "otherprop1" },
                new ExampleClass2{ Match = "match1", OtherProp = "otherprop2" },
                new ExampleClass2{ Match = "match1", OtherProp = "otherprop3" }
            }
        };
    }

    public class ExampleClass1
    {
        public string? Match { get; set; }
        public string? SomethingElse { get; set; }
    }

    public class ExampleClass2
    {
        public string? Match { get; set; }
        public string? OtherProp { get; set; }
    }
}
