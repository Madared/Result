namespace Results;

public class IntermediateContextResult<TOut> : IContextResultWithData<TOut> where TOut : notnull {
    private readonly Result<TOut> _result;
    private readonly IContextResultCallable<TOut> _callable;
    private readonly IContextResult _previousContext;
    
    public IntermediateContextResult(Result<TOut> result, IContextResultCallable<TOut> callable, IContextResult previousContext) {
        _result = result;
        _callable = callable;
        _previousContext = previousContext;
    }

    public bool Succeeded => _result.Succeeded;
    public bool Failed => _result.Failed;
    public IError Error => _result.Error;
    public TOut Data => _result.Data;
    public IContextResultWithData<TOut> Retry() {
        if (Succeeded) return this;
        if (_previousContext.Succeeded) {
            return new IntermediateContextResult<TOut>(_callable.Call(), _callable, _previousContext);
        }
        IContextResult newContext = _previousContext.Retry();
        return newContext.Failed 
            ? new IntermediateContextResult<TOut>(Result<TOut>.Fail(newContext.Error), _callable, newContext) 
            : new IntermediateContextResult<TOut>(_callable.Call(), _callable, newContext);
    }
    public Result<TOut> StripContext() => _result;
}