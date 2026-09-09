namespace Pidp.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Pharmacy : BaseAuditable
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Fax { get; set; } = string.Empty;
    public string PharmaCareCode { get; set; } = string.Empty;

    public ICollection<PharmacyPartyRole> Staff { get; set; } = [];
    public ICollection<PharmacyEnrolment> EnrolmentLinks { get; set; } = [];

    [ForeignKey(nameof(Manager))]
    public int? ManagerId { get; set; }
    public Party? Manager { get; set; }
}