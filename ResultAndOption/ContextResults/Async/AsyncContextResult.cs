namespace Results;

public class AsyncContextResult<TIn, TOut> where TIn : notnull where TOut : notnull {
    private AsyncContext<TIn, TOut>? _previousContext;
    private 

    public async Task<AsyncContextResult<TIn, TOut>> ReRun() => throw new NotImplementedException();
}

public class AsyncContext<TIn, TOut> where TIn : notnull where TOut : notnull {
    private Option<AsyncContextResult<TIn, TOut>> _async;
    private Option<ContextResult<TIn, TOut>> _sync;

    private AsyncContext(Option<AsyncContextResult<TIn, TOut>> async, Option<ContextResult<TIn, TOut>> sync) {
        _sync = sync;
        _async = async;
    }

    public async Task<AsyncContext<TIn, TOut>> Retry() {
        if (_async.IsNone() && _sync.IsNone()) throw new InvalidDataException();
        if (_sync.IsSome()) {
            return Create(_sync.Data.ReRun());
        }
        AsyncContextResult<TIn, TOut> retried = await _async.Data.ReRun();
        return Create(retried);
    }

    public static AsyncContext<TIn, TOut> Create(AsyncContextResult<TIn, TOut> async) {
        return new AsyncContext<TIn, TOut>(async.ToOption(), Option<ContextResult<TIn, TOut>>.None());
    }

    public static AsyncContext<TIn, TOut> Create(ContextResult<TIn, TOut> sync) {
        return new AsyncContext<TIn, TOut>(Option<AsyncContextResult<TIn, TOut>>.None(), sync.ToOption());
    }
}