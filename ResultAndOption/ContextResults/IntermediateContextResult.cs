namespace Results;

public class IntermediateContextResult<TOut> : IContextResult<TOut> where TOut : notnull {
    private readonly Result<TOut> _result;
    private readonly Func<Result<TOut>> _callable;
    private readonly IContextResult _previousContext;
    
    private IntermediateContextResult(Result<TOut> result, Func<Result<TOut>> callable, IContextResult previousContext) {
        _result = result;
        _callable = callable;
        _previousContext = previousContext;
    }

    public bool Succeeded => _result.Succeeded;
    public bool Failed => _result.Failed;
    public IError Error => _result.Error;
    public TOut Data => _result.Data;

    IContextResult<TOut> IContextResult<TOut>.Retry() => Retry();
    public IntermediateContextResult<TOut> Retry() {
        if (Succeeded) return this;
        IContextResult newContext = _previousContext.Retry();
        return newContext.Failed 
            ? new IntermediateContextResult<TOut>(Result<TOut>.Fail(newContext.Error), _callable, newContext) 
            : new IntermediateContextResult<TOut>(_callable(), _callable, newContext);
    }
    public Result<TOut> StripContext() => _result;
    public IContextResult<TNext> Map<TNext>(Func<TOut, Result<TNext>> mapper) where TNext : notnull {
        throw new NotImplementedException();
    }

    public IContextResult<TNext> Map<TNext>(Func<TOut, TNext> mapper) where TNext : notnull {
        throw new NotImplementedException();
    }

    public IContextResult Map(Func<TOut, Result> mapper) {
        throw new NotImplementedException();
    }

    public IContextResult Map(Action<TOut> mapper) {
        throw new NotImplementedException();
    }

    public IContextResult Map(Action action) {
        throw new NotImplementedException();
    }

    public IContextResult Map(Func<Result> mapper) {
        throw new NotImplementedException();
    }

    public IContextResult<TOut1> Map<TOut1>(Func<Result<TOut1>> mapper) where TOut1 : notnull {
        throw new NotImplementedException();
    }

    public IContextResult<TOut1> Map<TOut1>(Func<TOut1> mapper) where TOut1 : notnull {
        throw new NotImplementedException();
    }
    public static IntermediateContextResult<TOut> Create(Func<Result<TOut>> function, IContextResult previousContext) => previousContext.Failed 
        ? new IntermediateContextResult<TOut>(Result<TOut>.Fail(previousContext.Error), function, previousContext) 
        : new IntermediateContextResult<TOut>(function(), function, previousContext);
}