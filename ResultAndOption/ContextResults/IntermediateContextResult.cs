using System.CodeDom.Compiler;
using Results.ContextResultExtensions;

namespace Results;

internal class IntermediateContextResult<TOut> : IContextResult<TOut> where TOut : notnull {
    private readonly Result<TOut> _result;
    private readonly Func<Result<TOut>> _callable;
    private readonly IContextResult _previousContext;
    private readonly ResultEmitter<TOut> _resultEmitter = new();

    public IntermediateContextResult(Result<TOut> result, Func<Result<TOut>> callable, IContextResult previousContext) {
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
        if (newContext.Failed) return new IntermediateContextResult<TOut>(Result<TOut>.Fail(newContext.Error), _callable, newContext);
        Result<TOut> newResult = _callable();
        _resultEmitter.SetResult(newResult);
        return new IntermediateContextResult<TOut>(_callable(), _callable, newContext);
    }

    public Result<TOut> StripContext() => _result;

    public IContextResult<TNext> Map<TNext>(Func<TOut, Result<TNext>> mapper) where TNext : notnull => Failed
        ? new ContextResult<TOut, TNext>(mapper, this, Result<TNext>.Fail(Error))
        : new ContextResult<TOut, TNext>(mapper, this, mapper(Data));


    public IContextResult<TOut> Do(Func<TOut, Result> mapper) {
        return Failed
            ? new IntermediateContextResultSimple<TOut>(mapper, this, Result.Fail(Error)).Map(() => _result)
            : new IntermediateContextResultSimple<TOut>(mapper, this, mapper(Data)).Map(() => _result);
    }

    public IContextResult<TOut> Do(Action<TOut> action) => action.WrapInResult().Pipe(Do);

    public IContextResult<TOut> Do(Func<Result> action) {
        ResultSubscriber<TOut> subscriber = new(_result);
        _resultEmitter.Subscribe(subscriber);
        return Failed
            ? new SimpleContextResult(this, action, Result.Fail(Error)).Map(() => subscriber.Result)
            : new SimpleContextResult(this, action, action()).Map(() => subscriber.Result);
    }

    public IContextResult<TNext> Map<TNext>(Func<Result<TNext>> mapper) where TNext : notnull => Failed
        ? new IntermediateContextResult<TNext>(Result<TNext>.Fail(Error), mapper, this)
        : new IntermediateContextResult<TNext>(mapper(), mapper, this);

    public IContextResult<TNext> Map<TNext>(Func<TNext> mapper) where TNext : notnull => Map(mapper.WrapInResult());
    public IContextResult<TOut> Do(Action action) => Do(action.WrapInResult());
    public IContextResult Map(Action<TOut> mapper) => Do(mapper.WrapInResult());

    public IContextResult<TNext> Map<TNext>(Func<TOut, TNext> mapper) where TNext : notnull => Map(mapper.WrapInResult());
}