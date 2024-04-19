namespace Results.ContextResultExtensions;

internal sealed class CommandWithCallInputWrapperGenerator<T> : ICommandGenerator where T : notnull {
    private readonly ResultSubscriber<T> _subscriber;
    private readonly ICommandWithCallInput<T> _commandWithCallInput;

    public CommandWithCallInputWrapperGenerator(ResultSubscriber<T> subscriber, ICommandWithCallInput<T> commandWithCallInput) {
        throw new NotImplementedException();
    }

    public ICommand Generate() => new CommandWithCallInputWrapper<T>(_commandWithCallInput, _subscriber.Result);
}