namespace Pidp.Extensions;

using DomainResults.Common;

public static class DomainTaskExtensions
{
    public static async Task<IDomainResult> IfSuccess(this Task<IDomainResult> firstTask, Func<Task<IDomainResult>> taskIfSuccess)
    {
        var result = await firstTask;
        return result.IsSuccess
            ? await taskIfSuccess()
            : result;
    }

    public static async Task<IDomainResult<T>> IfSuccess<T>(this Task<IDomainResult> firstTask, Func<Task<IDomainResult<T>>> taskIfSuccess)
    {
        var result = await firstTask;
        return result.IsSuccess
            ? await taskIfSuccess()
            : result.To<T>();
    }
}
