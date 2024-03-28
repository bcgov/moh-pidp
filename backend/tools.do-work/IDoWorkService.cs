namespace DoWork;

public interface IDoWorkService
{
    /// <summary>
    /// A service with DI that can be updated to perform manual tasks.
    /// </summary>
    public Task DoWorkAsync();
}
