using System.Net.Http.Headers;

namespace Results.ContextResults.Async;

public class AsyncContextResult<TIn, TOut> : IAsyncContextResultWithData<TIn, TOut> where TIn : notnull where TOut : notnull {
    private readonly IAsyncContextResult? _previousContext;
    private readonly IACRCallable<TOut> _callable;
    private readonly IError? _error;
    private readonly Option<TOut> _data;

    public bool Succeeded { get; }
    public bool Failed => !Succeeded;
    public IError Error => _error ?? throw new InvalidOperationException();
    public TOut Data => _data.IsSome() ? _data.Data : throw ErrorToExceptionMapper.Map(_error);

    public AsyncContextResult(IAsyncContextResult? previousContext, IACRCallable<TOut> callable, IError? error, Option<TOut> data, bool succeeded) {
        _previousContext = previousContext;
        _callable = callable;
        _error = error;
        _data = data;
        Succeeded = succeeded;
    }

    public Task<Result<TOut>> StripContext() => throw new NotImplementedException();

    public async Task<AsyncContextResult<TIn, TOut>> Retry() => throw new NotImplementedException();
    public Task<AsyncContextResult<TOut, TNext>> Map<TNext>(Func<TOut, Task<TNext>> mapper) where TNext : notnull => throw new NotImplementedException();
    public Task<AsyncContextResult<TOut, TNext>> Map<TNext>(Func<TOut, Task<Result<TNext>>> mapper) where TNext : notnull => throw new NotImplementedException();
    public Task<AsyncContextResult<TOut, TNext>> Map<TNext>(Func<TOut, TNext> mapper) where TNext : notnull => throw new NotImplementedException();
    public Task<ContextResultWrapperAsync<TOut, TNext>> Map<TNext>(Func<TOut, Result<TNext>> mapper) where TNext : notnull => throw new NotImplementedException();

    async Task<IAsyncContextResultWithData<TIn, TOut>> IAsyncContextResultWithData<TIn, TOut>.Retry() => await Retry();
}