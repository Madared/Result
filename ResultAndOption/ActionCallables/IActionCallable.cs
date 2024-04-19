namespace Results.ActionCallables;

public interface IActionCallable {
    void Call();
}

public interface IActionCallableGenerator {
    IActionCallable Generate();
}

internal sealed class ActionCallableGenerator : IActionCallableGenerator {
    private readonly Action _action;
    public ActionCallableGenerator(Action action) {
        _action = action;
    }
    public IActionCallable Generate() => new ActionCallable(_action);
}


internal sealed class ActionCallable : IActionCallable {
    private readonly Action _action;
    public ActionCallable(Action action) {
        _action = action;
    }
    public void Call() => _action();
}

internal sealed class ActionCallableWithInputGenerator<TIn> : IActionCallableGenerator where TIn : notnull {
    private readonly ResultSubscriber<TIn> _subscriber;
    private readonly Action<TIn> _action;
    
    public ActionCallableWithInputGenerator(ResultSubscriber<TIn> subscriber, Action<TIn> action) {
        _subscriber = subscriber;
        _action = action;
    }
    public IActionCallable Generate() => new ActionCallableWithInput<TIn>(_subscriber.Result, _action);
}

internal sealed class ActionCallableWithInput<TIn> : IActionCallable where TIn : notnull {
    private readonly Result<TIn> _result;
    private readonly Action<TIn> _action;
    public ActionCallableWithInput(Result<TIn> result, Action<TIn> action) {
        _result = result;
        _action = action;
    }
    public void Call() {
        if (_result.Failed) return;
        _action(_result.Data);
    }
}