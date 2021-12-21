namespace Pidp.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Pidp.Models.Lookups;

public enum AddressType
{
    Physical = 1,
    Mailng,
    Verified
}

[Table(nameof(Address))]
public abstract class Address : BaseAuditable
{
    [Key]
    public int Id { get; set; }

    public AddressType AddressType { get; set; }

    public CountryCode CountryCode { get; set; }

    public Country? Country { get; set; }

    public ProvinceCode ProvinceCode { get; set; }

    public Province? Province { get; set; }

    public string Street { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string Postal { get; set; } = string.Empty;
}

public class PartyAddress : Address
{
    public int PartyId { get; set; }

    public Party? Party { get; set; }
}

public class FacilityAddress : Address
{
    public int FacilityId { get; set; }

    public Facility? Facility { get; set; }
}
