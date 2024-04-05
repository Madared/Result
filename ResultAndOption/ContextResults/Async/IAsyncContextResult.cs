namespace Results.ContextResults.Async;

public interface IAsyncContextResult : IResult {
    Result StripContext();
    Task<IAsyncContextResult> Retry();
    Task<IAsyncContextResult> ReRun();
}