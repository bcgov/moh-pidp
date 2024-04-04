namespace Pidp.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Pidp.Models.Lookups;

[Table(nameof(Address))]
public abstract class Address : BaseAuditable
{
    [Key]
    public int Id { get; set; }

    public string CountryCode { get; set; } = string.Empty;

    public Country? Country { get; set; }

    public string ProvinceCode { get; set; } = string.Empty;

    public Province? Province { get; set; }

    public string Street { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string Postal { get; set; } = string.Empty;
}

public class MSTeamsClinicAddress : Address
{
    public int ClinicId { get; set; }

    public MSTeamsClinic? Clinic { get; set; }
}
