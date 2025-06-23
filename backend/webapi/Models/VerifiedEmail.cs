namespace Pidp.Models;

using EntityFrameworkCore.Projectables;
using NodaTime;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


[Table(nameof(VerifiedEmail))]
public class VerifiedEmail : BaseAuditable
{
    [Key]
    public int Id { get; set; }

    public int PartyId { get; set; }

    public Party? Party { get; set; }

    public Guid Token { get; set; }

    public string Email { get; set; } = string.Empty;

    public Instant? VerifiedOn { get; set; }

    [Projectable]
    public bool IsVerified => this.VerifiedOn != null;
}
