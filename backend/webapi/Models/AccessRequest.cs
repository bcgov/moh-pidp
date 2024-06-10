namespace Pidp.Models;

using NodaTime;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Pidp.Models.Lookups;

[Table(nameof(AccessRequest))]
public class AccessRequest : BaseAuditable
{
    [Key]
    public int Id { get; set; }

    public int PartyId { get; set; }

    public Party? Party { get; set; }

    public Instant RequestedOn { get; set; }

    public AccessTypeCode AccessTypeCode { get; set; }
}

[Table(nameof(HcimAccountTransfer))]
public class HcimAccountTransfer : AccessRequest
{
    public string LdapUsername { get; set; } = string.Empty;
}

[Table(nameof(MSTeamsClinicMemberEnrolment))]
public class MSTeamsClinicMemberEnrolment : AccessRequest
{
    public int ClinicId { get; set; }

    public MSTeamsClinic? Clinic { get; set; }
}
