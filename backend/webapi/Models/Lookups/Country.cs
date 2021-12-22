namespace Pidp.Models.Lookups;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public enum CountryCode
{
    Canada = 1,
    UnitedStates
}

[Table("CountryLookup")]
public class Country
{
    [Key]
    public CountryCode Code { get; set; }

    public string IsoCode { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
}

public class CountryDataGenerator : ILookupDataGenerator<Country>
{
    public IEnumerable<Country> Generate() => new[]
    {
        new Country { Code = CountryCode.Canada,       IsoCode = "CA", Name = "Canada"        },
        new Country { Code = CountryCode.UnitedStates, IsoCode = "US", Name = "United States" }
    };
}
