
using Results.ContextResultExtensions;

namespace Results;

internal class StartingContextResult<TOut> : IContextResult<TOut> where TOut : notnull {
    private readonly Result<TOut> _result;
    private readonly Func<Result<TOut>> _callable;
    private readonly ResultEmitter<TOut> _emitter;

    public StartingContextResult(Result<TOut> result, Func<Result<TOut>> callable) {
        _result = result;
        _callable = callable;
        _emitter = new();
    }

    public bool Succeeded => _result.Succeeded;
    public bool Failed => _result.Failed;
    public IError Error => _result.Error;
    public TOut Data => _result.Data;

    public IContextResult<TOut> Retry() {
        if (Succeeded) return this;
        Result<TOut> newResult = _callable();
        _emitter.Emit(newResult);
        return new StartingContextResult<TOut>(newResult, _callable);
    }

    public Result<TOut> StripContext() => _result;


    public IContextResult<TNext> Map<TNext>(Func<TOut, Result<TNext>> mapper) where TNext : notnull => Failed
        ? new ContextResult<TOut, TNext>(mapper, this, Result<TNext>.Fail(_result.Error))
        : new ContextResult<TOut, TNext>(mapper, this, mapper(_result.Data));

    public IContextResult<TOut> Do(Func<TOut, Result> mapper) => Failed
        ? new IntermediateContextResultSimple<TOut>(mapper, this, Result.Fail(Error)).Map(() => _result)
        : new IntermediateContextResultSimple<TOut>(mapper, this, mapper(Data)).Map(() => _result);


    public IContextResult<TOut> Do(Action<TOut> action) {
        Func<TOut, Result> generated = action.WrapInResult();
        return Failed
            ? new IntermediateContextResultSimple<TOut>(generated, this, Result.Fail(Error)).Map(() => _result)
            : new IntermediateContextResultSimple<TOut>(generated, this, generated(Data)).Map(() => _result);
    }

    public IContextResult<TOut> Do(Func<Result> action) {
        ResultSubscriber<TOut> subscriber = new(_result);
        _emitter.Subscribe(subscriber);
        return Failed 
            ? new SimpleContextResult(this, action, Result.Fail(Error)).Map(() => subscriber.Result) 
            : new SimpleContextResult(this, action, action()).Map(() => subscriber.Result);
    }

    public IContextResult<TNext> Map<TNext>(Func<Result<TNext>> mapper) where TNext : notnull => Failed
        ? new IntermediateContextResult<TNext>(Result<TNext>.Fail(Error), mapper, this)
        : new IntermediateContextResult<TNext>(mapper(), mapper, this);

    public IContextResult<TNext> Map<TNext>(Func<TNext> mapper) where TNext : notnull => Map(mapper.WrapInResult());
    public IContextResult Map(Action<TOut> mapper) => Do(mapper.WrapInResult());
    public IContextResult<TOut> Do(Action action) => Do(action.WrapInResult());
    public IContextResult<TNext> Map<TNext>(Func<TOut, TNext> mapper) where TNext : notnull => Map(mapper.WrapInResult());
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

    public IContextResult Do(Func<Result> action) =>
        Failed
            ? new SimpleContextResult(this, action, Result.Fail(Error))
            : new SimpleContextResult(this, action, action());

    public IContextResult<TOut> Map<TOut>(Func<Result<TOut>> mapper) where TOut : notnull => Failed
        ? new IntermediateContextResult<TOut>(Result<TOut>.Fail(Error), mapper, this)
        : new IntermediateContextResult<TOut>(mapper(), mapper, this);

    public IContextResult Do(Action action) => Do(action.WrapInResult());
    public IContextResult<TOut> Map<TOut>(Func<TOut> mapper) where TOut : notnull => Map(mapper.WrapInResult());
}