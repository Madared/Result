namespace Results.ContextResults.Async;

public interface IAsyncContextResult : IResult {
    Task<Result> StripContext();
    Task<IAsyncContextResult> Retry();
}