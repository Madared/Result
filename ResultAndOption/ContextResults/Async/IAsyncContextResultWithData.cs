namespace Results.ContextResults.Async;

public interface IAsyncContextResultWithData<TIn, TOut> : IAsyncContextResult, IResultWithData<TOut> where TOut : notnull {
    Task<IAsyncContextResultWithData<TIn, TOut>> Retry();
    Result<TOut> StripContext();

    async Task<IAsyncContextResult> IAsyncContextResult.Retry() => await Retry();
    Result IAsyncContextResult.StripContext() => StripContext().ToSimpleResult();
}