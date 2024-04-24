using ResultAndOption.Results;

namespace Results.AsyncContext.AsyncContext;

public interface IAsyncCommand {
    Task<Result> Do();
    Task Undo();
}