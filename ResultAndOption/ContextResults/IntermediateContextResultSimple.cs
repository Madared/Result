namespace Results;

internal class IntermediateContextResultSimple<TIn> : IContextResult where TIn : notnull {
    private readonly IContextResult<TIn> _previousContext;
    private readonly Func<TIn, Result> _callable;
    private readonly Result _result;

    public IntermediateContextResultSimple(Func<TIn, Result> callable, IContextResult<TIn> previousContext, Result result) {
        _previousContext = previousContext;
        _callable = callable;
        _result = result;
    }

    public bool Succeeded => _result.Succeeded;
    public bool Failed => _result.Failed;
    public IError Error => _result.Error;

    IContextResult IContextResult.Retry() => Retry();

    public IntermediateContextResultSimple<TIn> Retry() {
        if (Succeeded) return this;
        IContextResult<TIn> retried = _previousContext.Retry();
        return retried.Succeeded 
            ? new IntermediateContextResultSimple<TIn>(_callable, _previousContext, _callable(retried.Data)) 
            : new IntermediateContextResultSimple<TIn>(_callable, _previousContext, Result.Fail(_previousContext.Error));
    }

    public Result StripContext() => _result;

    public IContextResult Map(Func<Result> mapper) => Failed
        ? new SimpleContextResult(this, mapper, Result.Fail(Error))
        : new SimpleContextResult(this, mapper, mapper());

    public IContextResult<TOut> Map<TOut>(Func<Result<TOut>> mapper) where TOut : notnull => Failed
        ? new IntermediateContextResult<TOut>(Result<TOut>.Fail(Error), mapper, this)
        : new IntermediateContextResult<TOut>(mapper(), mapper, this);

    public IContextResult<TOut> Map<TOut>(Func<TOut> mapper) where TOut : notnull => Map(mapper.WrapInResult());
    public IContextResult Map(Action action) => Map(action.WrapInResult());
}