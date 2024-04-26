using ResultAndOption.Results;

namespace Results.Context.ContextCommands;

internal sealed class CommandWithCallInputWrapper<T> : ICommand where T : notnull
{
    private readonly ICommandWithCallInput<T> _commandWithCallInput;
    private readonly Result<T> _result;

    public CommandWithCallInputWrapper(ICommandWithCallInput<T> commandWithCallInput, Result<T> result)
    {
        _commandWithCallInput = commandWithCallInput;
        _result = result;
    }

    public Result Call()
    {
        return _result.Failed ? Result.Fail(_result.Error) : _commandWithCallInput.Call(_result.Data);
    }

    public void Undo()
    {
        _commandWithCallInput.Undo();
    }
}