namespace Pidp.Models;

public interface IOwnedResource
{
    Guid UserId { get; set; }
}
