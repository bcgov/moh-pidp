namespace Pidp.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Pidp.Models.Lookups;

[Table(nameof(PartyOrgainizationDetail))]
public class PartyOrgainizationDetail : BaseAuditable
{
    [Key]
    public int Id { get; set; }

    public int PartyId { get; set; }

    public Party? Party { get; set; }

    public OrganizationCode OrganizationCode { get; set; }

    public HealthAuthorityCode HealthAuthorityCode { get; set; }

    public string EmployeeId { get; set; } = string.Empty;
}
