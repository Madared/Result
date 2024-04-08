namespace Results;

public class SimpleContextResult : IContextResult {
    private readonly IContextResult _previousContext;
    private readonly Func<Result> _callable;
    private readonly Result _result;
    public bool Succeeded => _result.Succeeded;
    public bool Failed => _result.Failed;
    public IError Error => _result.Error;

    private SimpleContextResult(IContextResult previousContext, Func<Result> callable, Result result) {
        _previousContext = previousContext;
        _callable = callable;
        _result = result;
    }

    IContextResult IContextResult.Retry() => Retry();
    public SimpleContextResult Retry() {
        if (Succeeded) return this;
        if (_previousContext.Succeeded) {
            Result output2 = _callable();
            return new SimpleContextResult(_previousContext, _callable, output2);
        }

        IContextResult retried = _previousContext.Retry();
        return retried.Failed
            ? new SimpleContextResult(retried, _callable, retried.StripContext())
            : new SimpleContextResult(retried, _callable, _callable());
    }
    public Result StripContext() => _result;
    public IContextResult Map(Action action) {
        throw new NotImplementedException();
    }

    public IContextResult Map(Func<Result> mapper) {
        throw new NotImplementedException();
    }

    public IContextResult<TOut> Map<TOut>(Func<Result<TOut>> mapper) where TOut : notnull {
        throw new NotImplementedException();
    }

    public IContextResult<TOut> Map<TOut>(Func<TOut> mapper) where TOut : notnull {
        throw new NotImplementedException();
    }
}