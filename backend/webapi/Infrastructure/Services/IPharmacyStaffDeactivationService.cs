namespace Pidp.Infrastructure.Services;

using System.Threading;
using System.Threading.Tasks;

public interface IPharmacyStaffDeactivationService
{
    Task DeactivateExpiredStaffAsync(CancellationToken cancellationToken);
}
