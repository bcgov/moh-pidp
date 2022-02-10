namespace PlrIntake.Features;

public interface IQueryHandler<TQuery, TResult> : IRequestHandler where TQuery : IQuery<TResult>
{
    Task<TResult> HandleAsync(TQuery query);
}
