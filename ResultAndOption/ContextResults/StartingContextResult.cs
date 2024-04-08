namespace Results;

public class StartingContextResult<TOut> : IContextResult<TOut> where TOut : notnull {
    private readonly Result<TOut> _result;
    private readonly Func<Result<TOut>> _callable;

    public StartingContextResult(Result<TOut> result, Func<Result<TOut>> callable) {
        _result = result;
        _callable = callable;
    }

    public bool Succeeded => _result.Succeeded;
    public bool Failed => _result.Failed;
    public IError Error => _result.Error;
    public TOut Data => _result.Data;

    public IContextResult<TOut> Retry() {
        throw new NotImplementedException();
    }

    public Result<TOut> StripContext() {
        throw new NotImplementedException();
    }

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

    public IContextResult<TNext> Map<TNext>(Func<Result<TNext>> mapper) where TNext : notnull {
        throw new NotImplementedException();
    }

    public IContextResult<TNext> Map<TNext>(Func<TNext> mapper) where TNext : notnull {
        throw new NotImplementedException();
    }

    public static StartingContextResult<TOut> Create(Func<Result<TOut>> function) => new (function(), function);
}

public class StartingContextResult : IContextResult {
    private readonly Result _result;
    private readonly Func<Result> _callable;

    public StartingContextResult(Result result, Func<Result> callable) {
        _result = result;
        _callable = callable;
    }

    public bool Succeeded => _result.Succeeded;
    public bool Failed => _result.Succeeded;
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

    public static StartingContextResult Create(Func<Result> function) => new(function(), function);
}