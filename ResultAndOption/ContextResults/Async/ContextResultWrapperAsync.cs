namespace Results.ContextResults.Async;

public class ContextResultWrapperAsync<TIn, TOut> : IAsyncContextResultWithData<TIn, TOut> where TIn : notnull where TOut : notnull {
    private readonly IContextResultWithData<TOut> _syncContext;

    public bool Succeeded => _syncContext.Succeeded;
    public bool Failed => _syncContext.Failed;
    public IError Error => _syncContext.Error;
    public TOut Data => _syncContext.Data;

    public ContextResultWrapperAsync(IContextResultWithData<TOut> syncContext) {
        _syncContext = syncContext;
    }

    public Result<TOut> StripContext() => _syncContext.StripContext();

    public Task<IAsyncContextResultWithData<TIn, TOut>> Retry() {
        IContextResultWithData<TOut> newContext = _syncContext.Retry();
        IAsyncContextResultWithData<TIn, TOut> wrapped = new ContextResultWrapperAsync<TIn, TOut>(newContext);
        return Task.FromResult(wrapped);
    }
}