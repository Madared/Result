namespace Results.ContextResultExtensions;

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