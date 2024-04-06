namespace Results;

public interface ISimpleContextResultCallable {
    public Result Call();
}

public class SimpleCRC : IContextResult {
    private readonly IContextResult _previousContext;
    private readonly ISimpleContextResultCallable _callable;
    private readonly IError? _error;
    public bool Succeeded { get; }
    public bool Failed => !Succeeded;
    public IError Error { get; }

    private SimpleCRC(IContextResult previousContext, ISimpleContextResultCallable callable, bool succeeded, IError? error) {
        _previousContext = previousContext;
        _callable = callable;
        Succeeded = succeeded;
        _error = error;
    }

    public IContextResult Retry() {
        if (Succeeded) return this;
        if (_previousContext.Failed) {
            IContextResult retried = _previousContext.Retry();
            if (retried.Failed) {
                return new SimpleCRC(retried, _callable, false, retried.Error);
            }

            Result output = _callable.Call();
            return output.Failed
                ? new SimpleCRC(retried, _callable, false, output.Error)
                : new SimpleCRC(retried, _callable, true, null);
        }

        Result output2 = _callable.Call();
        return output2.Failed
            ? new SimpleCRC(_previousContext, _callable, false, output2.Error)
            : new SimpleCRC(_previousContext, _callable, true, null);
    }

    public IContextResult Rerun() {
        throw new NotImplementedException();
    }
}