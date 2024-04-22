using Context.ContextResults;
using ResultAndOption;

namespace Context.ContextCommands;

internal sealed class CommandWithCallInputWrapperGenerator<T> : ICommandGenerator where T : notnull {
    private readonly ICommandWithCallInput<T> _commandWithCallInput;
    private readonly ResultSubscriber<T> _subscriber;

    public CommandWithCallInputWrapperGenerator(ResultSubscriber<T> subscriber, ICommandWithCallInput<T> commandWithCallInput) {
        throw new NotImplementedException();
    }

    public ICommand Generate() {
        return new CommandWithCallInputWrapper<T>(_commandWithCallInput, _subscriber.Result);
    }
}