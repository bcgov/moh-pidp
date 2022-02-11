namespace Pidp.Features;

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand> where TCommand : ICommand
{
    new Task HandleAsync(TCommand command);
}

public interface ICommandHandler<TCommand, TResult> : IRequestHandler<TCommand, TResult> where TCommand : ICommand<TResult>
{
    new Task<TResult> HandleAsync(TCommand command);
}
