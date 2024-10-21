namespace Pidp.Models;

using NodaTime;
using System.ComponentModel.DataAnnotations.Schema;

using Pidp.Models.DomainEvents;

public abstract class BaseEntity
{
    [NotMapped]
    public List<IDomainEvent> DomainEvents { get; set; } = [];
}

public abstract class BaseAuditable : BaseEntity
{
    public Instant Created { get; set; }
    public Instant Modified { get; set; }
}
