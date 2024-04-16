using System.Dynamic;
using Results.CallableGenerators;
using Results.ContextResultExtensions;

namespace Results;

internal class ContextResult<TIn, TOut> : IContextResult<TOut> where TIn : notnull where TOut : notnull {
    private readonly IContextResult<TIn> _previousContext;
    private readonly Func<TIn, Result<TOut>> _called;
    private readonly Result<TOut> _result;

    public IError Error => _result.Error;
    public TOut Data => _result.Data;
    public bool Succeeded => _result.Succeeded;
    public bool Failed => _result.Failed;

    public ContextResult(Func<TIn, Result<TOut>> called, IContextResult<TIn> previousContext, Result<TOut> result) {
        _called = called;
        _previousContext = previousContext;
        _result = result;
    }

    IContextResult<TOut> IContextResult<TOut>.Retry() => Retry();

    public Result<TOut> StripContext() => _result;

    public ContextResult<TIn, TOut> Retry() {
        if (Succeeded) return this;
        IContextResult<TIn> previousContext = _previousContext.Retry();
        return previousContext.Failed
            ? new ContextResult<TIn, TOut>(_called, previousContext, Result<TOut>.Fail(previousContext.Error))
            : new ContextResult<TIn, TOut>(_called, previousContext, _called(previousContext.Data));
    }

    public IContextResult<TNext> Map<TNext>(Func<TOut, Result<TNext>> mapper) where TNext : notnull => Failed
        ? new ContextResult<TOut, TNext>(mapper, this, Result<TNext>.Fail(Error))
        : new ContextResult<TOut, TNext>(mapper, this, mapper(Data));

    public IContextResult<TOut> Do(Func<TOut, Result> mapper) => Failed
        ? new IntermediateContextResultSimple<TOut>(mapper, this, Result.Fail(Error)).Map(() => _result)
        : new IntermediateContextResultSimple<TOut>(mapper, this, mapper(Data)).Map(() => _result);

    public IContextResult<TOut> Do(Func<Result> action) => Failed
        ? new SimpleContextResult(this, action, Result.Fail(Error)).Map(() => _result)
        : new SimpleContextResult(this, action, action()).Map(() => _result);

    public IContextResult<TOut> Do(Action<TOut> action) {
        Func<TOut, Result> generated = (TOut data) => {
            action(data);
            return Result.Ok();
        };
        return Failed
            ? new IntermediateContextResultSimple<TOut>(generated, this, Result.Fail(Error)).Map(() => _result)
            : new IntermediateContextResultSimple<TOut>(generated, this, generated(Data)).Map(() => _result);
    }

    public IContextResult<TNext> Map<TNext>(Func<Result<TNext>> mapper) where TNext : notnull => Failed
        ? new IntermediateContextResult<TNext>(Result<TNext>.Fail(Error), mapper, this)
        : new IntermediateContextResult<TNext>(mapper(), mapper, this);

    public IContextResult<TNext> Map<TNext>(Func<TNext> mapper) where TNext : notnull => Map(mapper.WrapInResult());
    public IContextResult<TOut> Do(Action action) => Do(action.WrapInResult());
    public IContextResult Map(Action<TOut> mapper) => Do(mapper.WrapInResult());
    public IContextResult<TNext> Map<TNext>(Func<TOut, TNext> mapper) where TNext : notnull => Map(mapper.WrapInResult());
}