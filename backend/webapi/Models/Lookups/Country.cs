namespace Pidp.Models.Lookups;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("CountryLookup")]
public class Country
{
    [Key]
    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
}

public class CountryDataGenerator : ILookupDataGenerator<Country>
{
    public IEnumerable<Country> Generate() =>
    [
        new Country { Code = "CA", Name = "Canada"        },
        new Country { Code = "US", Name = "United States" }
    ];
}
