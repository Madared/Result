namespace Results;

internal class StartingContextResult<TOut> : IContextResult<TOut> where TOut : notnull {
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

    public IContextResult<TOut> Retry() => Succeeded ? this : new StartingContextResult<TOut>(_callable(), _callable);

    public Result<TOut> StripContext() => _result;

    public IContextResult<TNext> Map<TNext>(Func<TOut, Result<TNext>> mapper) where TNext : notnull => Failed
        ? new ContextResult<TOut, TNext>(mapper, this, Result<TNext>.Fail(Error))
        : new ContextResult<TOut, TNext>(mapper, this, mapper(Data));


    public IContextResult Map(Func<TOut, Result> mapper) => Failed
        ? new IntermediateContextResultSimple<TOut>(mapper, this, Result.Fail(Error))
        : new IntermediateContextResultSimple<TOut>(mapper, this, mapper(Data));


    public IContextResult Map(Func<Result> mapper) => Failed
        ? new SimpleContextResult(this, mapper, Result.Fail(Error))
        : new SimpleContextResult(this, mapper, mapper());

    public IContextResult<TNext> Map<TNext>(Func<Result<TNext>> mapper) where TNext : notnull => Failed
        ? new IntermediateContextResult<TNext>(Result<TNext>.Fail(Error), mapper, this)
        : new IntermediateContextResult<TNext>(mapper(), mapper, this);

    public IContextResult<TNext> Map<TNext>(Func<TNext> mapper) where TNext : notnull => Map(mapper.WrapInResult());
    public IContextResult Map(Action<TOut> mapper) => Map(mapper.WrapInResult());
    public IContextResult Map(Action action) => Map(action.WrapInResult());
    public IContextResult<TNext> Map<TNext>(Func<TOut, TNext> mapper) where TNext : notnull => Map(mapper.WrapInResult());
    public static StartingContextResult<TOut> Create(Func<Result<TOut>> function) => new(function(), function);
}

internal class StartingContextResult : IContextResult {
    private readonly Result _result;
    private readonly Func<Result> _callable;

    public StartingContextResult(Result result, Func<Result> callable) {
        _result = result;
        _callable = callable;
    }

    public bool Succeeded => _result.Succeeded;
    public bool Failed => _result.Failed;
    public IError Error => _result.Error;

    public IContextResult Retry() => Succeeded ? this : new StartingContextResult(_callable(), _callable);

    public Result StripContext() => _result;

    public IContextResult Map(Func<Result> mapper) => Failed
        ? new SimpleContextResult(this, mapper, Result.Fail(Error))
        : new SimpleContextResult(this, mapper, mapper());

    public IContextResult<TOut> Map<TOut>(Func<Result<TOut>> mapper) where TOut : notnull => Failed
        ? new IntermediateContextResult<TOut>(Result<TOut>.Fail(Error), mapper, this)
        : new IntermediateContextResult<TOut>(mapper(), mapper, this);

    public IContextResult Map(Action action) => Map(action.WrapInResult());
    public IContextResult<TOut> Map<TOut>(Func<TOut> mapper) where TOut : notnull => Map(mapper.WrapInResult());

    public static StartingContextResult Create(Func<Result> function) => new(function(), function);
}