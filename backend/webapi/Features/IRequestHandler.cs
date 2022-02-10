namespace Pidp.Features;

public interface IRequestHandler
{
}

public interface IRequestHandler<TRequest> : IRequestHandler
{
    Task HandleAsync(TRequest request);
}

public interface IRequestHandler<TRequest, TResult> : IRequestHandler
{
    Task<TResult> HandleAsync(TRequest request);
}
