using System.Net.Http.Headers;

namespace Results;

public class ContextResult<TIn, TOut> : IContextResultWithData<TOut> where TIn : notnull where TOut : notnull {
    private readonly IContextResultWithData<TIn>? _previousContext;
    private readonly IContextResultCallable<TOut> _called;
    private readonly Result<TOut> _result;

    public IError Error => _result.Error;

    public TOut Data => _result.Data;

    public bool Succeeded => _result.Succeeded;
    public bool Failed => _result.Failed;

    public ContextResult(IContextResultCallable<TOut> called, IContextResultWithData<TIn>? previousContext, Result<TOut> result) {
        _called = called;
        _previousContext = previousContext;
        _result = result;
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
            ? Ok(_called, null, output.Data)
            : Fail(_called, null, output.Error);
    }

    private ContextResult<TIn, TOut> ReRunWithNewContext(IContextResultWithData<TIn> context) {
        if (_called is IContextResultCallableWithData<TIn, TOut> dataContext) {
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

    private static ContextResult<TIn, TOut> Ok(IContextResultCallable<TOut> callable, IContextResultWithData<TIn>? previousContext, TOut data) =>
            new ContextResult<TIn, TOut>(callable, previousContext, Result<TOut>.Ok(data));
        

    private static ContextResult<TIn, TOut> Fail(IContextResultCallable<TOut> callable, IContextResultWithData<TIn>? previousContext, IError error) =>
        new ContextResult<TIn, TOut>(callable, previousContext, Result<TOut>.Fail(error));

    public static ContextResult<TIn, TOut> Create(Func<Result<TOut>> function) {
        IContextResultCallable<TOut> callable = new CRCNoArguments<TOut>(function);
        Result<TOut> output = function();
        return output.Failed ? Fail(callable, null, output.Error) : Ok(callable, null, output.Data);
    }
}