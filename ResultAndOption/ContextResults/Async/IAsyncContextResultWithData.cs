namespace Results.ContextResults.Async;

public interface IAsyncContextResultWithData<TIn, TOut> : IAsyncContextResult, IResultWithData<TOut> where TOut : notnull {
    Task<IAsyncContextResultWithData<TIn, TOut>> Retry();
    Task<IAsyncContextResultWithData<TIn, TOut>> ReRun();
    Result<TOut> StripContext();

    async Task<IAsyncContextResult> IAsyncContextResult.Retry() => await Retry();
    async Task<IAsyncContextResult> IAsyncContextResult.ReRun() => await ReRun();
    Result IAsyncContextResult.StripContext() => StripContext().ToSimpleResult();
}