using System.Net.Http.Headers;

namespace Results;

public struct ContextResult<TIn, TOut> : IContextResultWithData<TOut> where TIn : notnull where TOut : notnull {
    private readonly IContextResultWithData<TIn>? _previousContext;
    private readonly IContextResultCallable<TOut> _called;
    private readonly Option<TOut> _data;
    private readonly IError? _error;

    public IError Error => _error ?? throw new InvalidOperationException("Cannot access error on successful result");
    public TOut Data => _data.IsNone() ? throw new InvalidOperationException() : _data.Data;
    public bool Succeeded { get; }
    public bool Failed => !Succeeded;

    public ContextResult(IContextResultCallable<TOut> called, Option<TOut> data, IContextResultWithData<TIn>? previousContext, IError? error, bool succeeded) {
        _called = called;
        _data = data;
        _error = error;
        _previousContext = previousContext;
        Succeeded = succeeded;
    }

    IContextResultWithData<TOut> IContextResultWithData<TOut>.Retry() => Retry();

    public ContextResult<TIn, TOut> Retry() {
        if (Succeeded) return this;
        if (_called is null) return this;
        if (_previousContext is null) return RetryWithoutContext();
        
        IContextResultWithData<TIn> previousContext = _previousContext.Retry();
        return previousContext.Failed
            ? Fail(_called, previousContext, previousContext.Error)
            : RetryWithNewContext(previousContext);
    }

    private ContextResult<TIn, TOut> RetryWithoutContext() {
        Result<TOut> output = _called.Call();
        return output.Succeeded
            ? Ok(_called, output.Data)
            : Fail(_called, output.Error);
    }

    private ContextResult<TIn, TOut> RetryWithNewContext(IContextResultWithData<TIn> context) {
        if (_called is IContextResultCallableWithData<TIn, TOut> dataContext) {
            IContextResultCallable<TOut> newCallable = dataContext.WithData(context.Data);
            Result<TOut> output = newCallable.Call();
            return output.Succeeded
                ? Ok(newCallable, context, output.Data)
                : Fail(newCallable, context, output.Error);
        }

        Result<TOut> noInputOutput = _called.Call();
        return noInputOutput.Succeeded
            ? Ok(_called, context, noInputOutput.Data)
            : Fail(_called, context, noInputOutput.Error);
    }

    public static ContextResult<TIn, TOut> Ok(IContextResultCallable<TOut> callable, TOut data) =>
        new ContextResult<TIn, TOut>(callable, data.ToOption(), null, null, true);

    public static ContextResult<TIn, TOut> Ok(IContextResultCallable<TOut> callable, IContextResultWithData<TIn> previousContext, TOut data) =>
        previousContext.Succeeded
            ? new ContextResult<TIn, TOut>(callable, data.ToOption(), previousContext, null, true)
            : throw new InvalidOperationException();

    public static ContextResult<TIn, TOut> Ok(TOut data) =>
        new ContextResult<TIn, TOut>(
            new SimpleContextResultCallable<TOut>(data),
            data.ToOption(),
            null,
            null,
            true
        );

    public static ContextResult<TIn, TOut> Fail(IContextResultCallable<TOut> callable, IContextResultWithData<TIn> previousContext, IError error) =>
        previousContext.Failed
            ? new ContextResult<TIn, TOut>(callable, Option<TOut>.None(), previousContext, error, false)
            : throw new InvalidOperationException();


    public static ContextResult<TIn, TOut> Fail(IContextResultCallable<TOut> callable, IError error) =>
        new ContextResult<TIn, TOut>(
            callable,
            Option<TOut>.None(),
            null,
            error,
            false
        );

    public static ContextResult<TIn, TOut> From(IContextResultCallable<TOut> callable) {
        Result<TOut> output = callable.Call();
        return output.Failed
            ? new ContextResult<TIn, TOut>(callable, Option<TOut>.None(), null, output.Error, false)
            : new ContextResult<TIn, TOut>(callable, output.Data.ToOption(), null, null, true);
    }

    public static ContextResult<TIn, TOut> From(IContextResultWithData<TIn> previousContext, Func<TIn, TOut> mapper) {
        if (previousContext.Failed) {
            IContextResultCallable<TOut> failureCallable = new ContextResultCallableNoArguments<TOut>(() => Result<TOut>.Fail(previousContext.Error));
            return new ContextResult<TIn, TOut>(failureCallable, Option<TOut>.None(), previousContext, previousContext.Error, false);
        }

        IContextResultCallable<TOut> successCallable = new ContextResultCallableOfNotNull<TIn, TOut>(mapper, previousContext.Data);
        Result<TOut> output = successCallable.Call();
        return output.Failed
            ? new ContextResult<TIn, TOut>(successCallable, Option<TOut>.None(), previousContext, output.Error, false)
            : new ContextResult<TIn, TOut>(successCallable, output.Data.ToOption(), previousContext, null, true);
    }

    public static ContextResult<TIn, TOut> From(IContextResultWithData<TIn> previousContext, Func<TIn, Result<TOut>> mapper) {
        if (previousContext.Failed) {
            IContextResultCallable<TOut> failureCallable = new ContextResultCallableNoArguments<TOut>(() => Result<TOut>.Fail(previousContext.Error));
            return new ContextResult<TIn, TOut>(failureCallable, Option<TOut>.None(), previousContext, previousContext.Error, false);
        }

        IContextResultCallable<TOut> successCallable = new ContextResultCallableOfResult<TIn, TOut>(previousContext.Data, mapper);
        Result<TOut> output = successCallable.Call();
        return output.Failed
            ? new ContextResult<TIn, TOut>(successCallable, Option<TOut>.None(), previousContext, output.Error, false)
            : new ContextResult<TIn, TOut>(successCallable, output.Data.ToOption(), previousContext, null, true);
    }


    public static ContextResult<TIn, TOut> FromCallable(Func<TIn, Result<TOut>> callable, TIn data) {
        IContextResultCallable<TOut> contextCallable = new ContextResultCallableOfResult<TIn, TOut>(data, callable);
        Result<TOut> output = callable(data);
        return output.Succeeded
            ? new ContextResult<TIn, TOut>(contextCallable, output.Data.ToOption(), null, null, true)
            : new ContextResult<TIn, TOut>(contextCallable, Option<TOut>.None(), null, output.Error, true);
    }

    public static ContextResult<TIn, TOut> FromCallable(Func<Result<TOut>> callable) {
        IContextResultCallable<TOut> contextResultCallable = new ContextResultCallableNoArguments<TOut>(callable);
        Result<TOut> output = callable();
        return output.Succeeded
            ? new ContextResult<TIn, TOut>(contextResultCallable, output.Data.ToOption(), null, null, true)
            : new ContextResult<TIn, TOut>(contextResultCallable, Option<TOut>.None(), null, output.Error, true);
    }
}