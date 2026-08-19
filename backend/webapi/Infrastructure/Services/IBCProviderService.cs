namespace Pidp.Infrastructure.Services;

using System.Threading;
using System.Threading.Tasks;

public interface IBCProviderService
{
    Task UpdatePharmStaffAttributes(int partyId, CancellationToken cancellationToken);
}
