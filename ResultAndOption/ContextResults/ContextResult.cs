using System.Net.Http.Headers;

namespace Results;

public class ContextResult<TIn, TOut> : IContextResultWithData<TOut> where TIn : notnull where TOut : notnull {
    private readonly IContextResultWithData<TIn>? _previousContext;
    private readonly IContextResultCallable<TOut> _called;
    private readonly Option<TOut> _data;
    private readonly IError? _error;

    public IError Error => _error ?? throw new InvalidOperationException("Cannot access error on successful result");

    public TOut Data => _data.IsNone()
        ? throw ErrorToExceptionMapper.Map(_error)
        : _data.Data;

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


    public Result<TOut> StripContext() => Succeeded ? Result<TOut>.Ok(Data) : Result<TOut>.Fail(Error); 

    public ContextResult<TIn, TOut> Retry() {
        if (Succeeded) return this;
        if (_called is null) return this;
        if (_previousContext is null) return ReRunWithoutContext();

        IContextResultWithData<TIn> previousContext = _previousContext.Retry();
        return previousContext.Failed
            ? Fail(_called, previousContext, previousContext.Error)
            : ReRunWithNewContext(previousContext);
    }

    private ContextResult<TIn, TOut> ReRunWithoutContext() {
        Result<TOut> output = _called.Call();
        return output.Succeeded
            ? Ok(_called, output.Data)
            : Fail(_called, output.Error);
    }

    private ContextResult<TIn, TOut> ReRunWithNewContext(IContextResultWithData<TIn> context) {
        if (_called is IContextResultCallableWithInput<TIn, TOut> dataContext) {
            IContextResultCallable<TOut> newCallable = dataContext.WithInput(context.Data);
            Result<TOut> output = newCallable.Call();
            return output.Succeeded
                ? Ok(newCallable, context, output.Data)
                : Fail(newCallable, context, Error);
        }

        Result<TOut> noInputOutput = _called.Call();
        return noInputOutput.Succeeded
            ? Ok(_called, context, noInputOutput.Data)
            : Fail(_called, context, noInputOutput.Error);
    }

    public ContextResult<TOut, TNext> Map<TNext>(Func<TOut, Result<TNext>> mapper) where TNext : notnull {
        if (Failed) {
            return ContextResult<TOut, TNext>.Fail(new ContextResultCallableNoArguments<TNext>(() => Result<TNext>.Fail(new UnknownError())), this, Error);
        }
        ContextResultCallableOfResult<TOut, TNext> callable = new(Data, mapper);
        return ContextResult<TOut, TNext>.Maybe(callable, this);
    }

    public ContextResult<TOut, TNext> Map<TNext>(Func<TOut, TNext> mapper) where TNext : notnull {
        if (Failed) {
            return ContextResult<TOut, TNext>.Fail(new ContextResultCallableNoArguments<TNext>(() => Result<TNext>.Fail(new UnknownError())), this, Error);
        }

        ContextResultCallableOfNotNull<TOut, TNext> callable = new(Data, mapper);
        return ContextResult<TOut, TNext>.Maybe(callable, this);
    }

    public static ContextResult<TIn, TOut> Maybe(IContextResultCallable<TOut> callable, IContextResultWithData<TIn> previousContext) {
        Result<TOut> callResult = callable.Call();
        return callResult.Failed 
            ? Fail(callable, previousContext, callResult.Error) 
            : Ok(callable, previousContext, callResult.Data);
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
        new ContextResult<TIn, TOut>(callable, Option<TOut>.None(), previousContext, error, false);


    public static ContextResult<TIn, TOut> Fail(IContextResultCallable<TOut> callable, IError error) =>
        new ContextResult<TIn, TOut>(
            callable,
            Option<TOut>.None(),
            null,
            error,
            false
        );
    public static ContextResult<TIn, TOut> FromCallable(Func<Result<TOut>> callable) {
        IContextResultCallable<TOut> contextResultCallable = new ContextResultCallableNoArguments<TOut>(callable);
        Result<TOut> output = callable();
        return output.Succeeded
            ? new ContextResult<TIn, TOut>(contextResultCallable, output.Data.ToOption(), null, null, true)
            : new ContextResult<TIn, TOut>(contextResultCallable, Option<TOut>.None(), null, output.Error, true);
    }
    
}