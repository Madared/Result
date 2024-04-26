using ResultAndOption.Results;

namespace Results.AsyncContext.AsyncContext.AsyncCommands;

public interface IAsyncCommand
{
    Task<Result> Do();
    Task Undo();
}