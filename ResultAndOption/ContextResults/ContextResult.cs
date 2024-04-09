using System.CodeDom.Compiler;
using System.Net.Http.Headers;

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
    public IContextResult Map(Action action) => Map(action.WrapInResult());
    public IContextResult Map(Action<TOut> mapper) => Map(mapper.WrapInResult());
    public IContextResult<TNext> Map<TNext>(Func<TOut, TNext> mapper) where TNext : notnull => Map(mapper.WrapInResult());
}