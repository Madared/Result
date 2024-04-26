using ResultAndOption.Results;
using Results.Context.ContextCallables;

namespace Results.Context.ContextCommands;

public interface ICommand : ICallable
{
    void Undo();
}

public interface ICommandWithInput<in T> where T : notnull
{
    Result Call(T data);
    void Undo(T data);
}

public interface ICommandWithUndoInput<in T> where T : notnull
{
    Result Call();
    void Undo(T data);
}

public interface ICommandWithCallInput<in T> where T : notnull
{
    Result Call(T data);
    void Undo();
}