namespace Results.ContextResults.Async;

public interface IAsyncContextResultWithData<TIn, TOut> : IAsyncContextResult, IResultWithData<TOut> where TOut : notnull {
    Task<IAsyncContextResultWithData<TIn, TOut>> Retry();
    Task<Result<TOut>> StripContext();

    async Task<IAsyncContextResult> IAsyncContextResult.Retry() => await Retry();
    async Task<Result> IAsyncContextResult.StripContext() => (await StripContext()).ToSimpleResult();
}