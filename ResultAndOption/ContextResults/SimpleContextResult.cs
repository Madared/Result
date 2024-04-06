namespace Results;

public class SimpleContextResult : IContextResult {
    private readonly IContextResult _previousContext;
    private readonly ISimpleContextResultCallable _callable;
    private readonly IResult _result;
    public bool Succeeded => _result.Succeeded;
    public bool Failed => _result.Failed;
    public IError Error => _result.Error;

    private SimpleContextResult(IContextResult previousContext, ISimpleContextResultCallable callable, IResult result) {
        _previousContext = previousContext;
        _callable = callable;
        _result = result;
    }

    public IContextResult Retry() {
        if (Succeeded) return this;
        if (_previousContext.Succeeded) {
            Result output2 = _callable.Call();
            return new SimpleContextResult(_previousContext, _callable, output2);
        }
        IContextResult retried = _previousContext.Retry();
        return retried.Failed
            ? new SimpleContextResult(retried, _callable, retried)
            : new SimpleContextResult(retried, _callable, _callable.Call());
    }
}