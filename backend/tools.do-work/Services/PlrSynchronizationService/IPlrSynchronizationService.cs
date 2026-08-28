namespace DoWork.Services.PlrSynchronizationService;

public interface IPlrSynchronizationService
{
    Task SynchronizePlrToEntraAsync(bool dryRun);
}
