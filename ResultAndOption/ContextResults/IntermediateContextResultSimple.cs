namespace Results;

public class IntermediateContextResultSimple<TIn> : IContextResult where TIn : notnull {
    private readonly IContextResult<TIn> _previousContext;
    private readonly Func<TIn, Result> _callable;
    private readonly Result _result;

    private IntermediateContextResultSimple(Func<TIn, Result> callable, IContextResult<TIn> previousContext, Result result) {
        _previousContext = previousContext;
        _callable = callable;
        _result = result;
    }
    public bool Succeeded => _result.Succeeded;
    public bool Failed => _result.Failed;
    public IError Error => _result.Error;
    public IContextResult Retry() {
        throw new NotImplementedException();
    }

    public Result StripContext() {
        throw new NotImplementedException();
    }

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

    public static IntermediateContextResultSimple<TIn> Create(Func<TIn, Result> function, IContextResult<TIn> previousContext) => previousContext.Failed
        ? new IntermediateContextResultSimple<TIn>(function, previousContext, Result.Fail(previousContext.Error))
        : new IntermediateContextResultSimple<TIn>(function, previousContext, function(previousContext.Data));
}