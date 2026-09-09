namespace Pidp.Infrastructure.Services;

using System.Threading;
using System.Threading.Tasks;

public interface IRoleSynchronizationService
{
    Task UpdatePharmStaffAttributes(int partyId, CancellationToken cancellationToken = default);
}
