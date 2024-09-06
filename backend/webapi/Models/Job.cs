namespace Pidp.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table(nameof(Job))]
public class Job
{
    [Key]
    public int Id { get; set; }

    public Guid UserId { get; set; }

    public string Email { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public bool Complete { get; set; }
}
