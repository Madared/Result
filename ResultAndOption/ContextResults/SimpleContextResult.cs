using System.Runtime.InteropServices.JavaScript;
using Results.ContextResultExtensions;

namespace Results;

internal class SimpleContextResult : IContextResult {
    private readonly IContextResult _previousContext;
    private readonly Func<Result> _callable;
    private readonly Result _result;
    public bool Succeeded => _result.Succeeded;
    public bool Failed => _result.Failed;
    public IError Error => _result.Error;

    public SimpleContextResult(IContextResult previousContext, Func<Result> callable, Result result) {
        _previousContext = previousContext;
        _callable = callable;
        _result = result;
    }

    public Result StripContext() => _result;
    IContextResult IContextResult.Retry() => Retry();

    public SimpleContextResult Retry() {
        if (Succeeded) return this;
        IContextResult retried = _previousContext.Retry();
        return retried.Failed
            ? new SimpleContextResult(retried, _callable, retried.StripContext())
            : new SimpleContextResult(retried, _callable, _callable());
    }

    public IContextResult Do(Func<Result> action) => Failed
        ? new SimpleContextResult(this, action, Result.Fail(Error))
        : new SimpleContextResult(this, action, action());

    public IContextResult<TOut> Map<TOut>(Func<Result<TOut>> mapper) where TOut : notnull => Failed
        ? new IntermediateContextResult<TOut>(Result<TOut>.Fail(Error), mapper, this)
        : new IntermediateContextResult<TOut>(mapper(), mapper, this);

    public IContextResult<TOut> Map<TOut>(Func<TOut> mapper) where TOut : notnull => Map(mapper.WrapInResult());
    public IContextResult Do(Action action) => Do(action.WrapInResult());
}