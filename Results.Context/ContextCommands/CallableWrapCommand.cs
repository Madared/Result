using ResultAndOption.Results;
using Results.Context.ActionCallables;
using Results.Context.ContextCallables;

namespace Results.Context.ContextCommands;

internal sealed class CallableWrapCommand : ICommand
{
    private readonly ICallable _callable;
    private readonly IActionCallable _undoer;

    public CallableWrapCommand(ICallable callable, IActionCallable undoer)
    {
        _callable = callable;
        _undoer = undoer;
    }

    public Result Call()
    {
        return _callable.Call();
    }

    public void Undo()
    {
        _undoer.Call();
    }
}