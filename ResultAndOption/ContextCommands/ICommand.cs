namespace Results.ContextResultExtensions;

public interface ICommand : IContextCallable {
    void Undo();
}

public interface ICommandWithInput<in T> where T : notnull {
    Result Call(T data);
    void Undo(T data);
}

public interface ICommandWithUndoInput<in T> where T : notnull {
    Result Call();
    void Undo(T data);
}

public interface ICommandWithCallInput<in T> where T : notnull {
    Result Call(T data);
    void Undo();
}

internal sealed class CommandWithInputWrapper<T> : ICommand where T : notnull {
    private readonly Result<T> _result;
    private readonly ICommandWithInput<T> _command;
    
    public CommandWithInputWrapper(Result<T> result, ICommandWithInput<T> command) {
        _result = result;
        _command = command;
    }
    public Result Call() => _result.Failed ? Result.Fail(_result.Error) : _command.Call(_result.Data);

    public void Undo() {
        if (_result.Failed) return;
        _command.Undo(_result.Data);
    }
}

internal sealed class CommandWithInputWrapperGenerator<T> : ICommandGenerator where T : notnull {
    private readonly ResultSubscriber<T> _subscriber;
    private readonly ICommandWithInput<T> _command;
    public CommandWithInputWrapperGenerator(ResultSubscriber<T> subscriber, ICommandWithInput<T> command) {
        _subscriber = subscriber;
        _command = command;
    }
    public ICommand Generate() => new CommandWithInputWrapper<T>(_subscriber.Result, _command);
}