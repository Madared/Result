using System.Net.Http.Headers;

namespace Results.ContextResults.Async;

public class AsyncContextResult<TIn, TOut> : IAsyncContextResultWithData<TIn, TOut> where TIn : notnull where TOut : notnull {
    private readonly IAsyncContextResult? _previousContext;
    private readonly IACRCallable<TOut> _callable;
    private readonly Result<TOut> _result;

    public bool Succeeded => _result.Succeeded;
    public bool Failed => _result.Failed;
    public IError Error => _result.Error;
    public TOut Data => _result.Data;

    public AsyncContextResult(IAsyncContextResult? previousContext, IACRCallable<TOut> callable, Result<TOut> result) {
        _previousContext = previousContext;
        _callable = callable;
        _result = result;
    }

    public Result<TOut> StripContext() => _result;

    public async Task<AsyncContextResult<TIn, TOut>> Retry() => throw new NotImplementedException();
    public Task<AsyncContextResult<TOut, TNext>> Map<TNext>(Func<TOut, Task<TNext>> mapper) where TNext : notnull => throw new NotImplementedException();
    public Task<AsyncContextResult<TOut, TNext>> Map<TNext>(Func<TOut, Task<Result<TNext>>> mapper) where TNext : notnull => throw new NotImplementedException();
    public Task<AsyncContextResult<TOut, TNext>> Map<TNext>(Func<TOut, TNext> mapper) where TNext : notnull => throw new NotImplementedException();
    public Task<ContextResultWrapperAsync<TOut, TNext>> Map<TNext>(Func<TOut, Result<TNext>> mapper) where TNext : notnull => throw new NotImplementedException();

    async Task<IAsyncContextResultWithData<TIn, TOut>> IAsyncContextResultWithData<TIn, TOut>.Retry() => await Retry();
}