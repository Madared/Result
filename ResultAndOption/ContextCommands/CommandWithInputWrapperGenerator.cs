namespace Results.ContextResultExtensions;

internal sealed class CommandWithInputWrapperGenerator<T> : ICommandGenerator where T : notnull {
    private readonly ResultSubscriber<T> _subscriber;
    private readonly ICommandWithInput<T> _command;

    public CommandWithInputWrapperGenerator(ResultSubscriber<T> subscriber, ICommandWithInput<T> command) {
        _subscriber = subscriber;
        _command = command;
    }

    public ICommand Generate() => new CommandWithInputWrapper<T>(_subscriber.Result, _command);
}