using System.Runtime.InteropServices.JavaScript;

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
        if (_previousContext.Succeeded) {
            Result output2 = _callable();
            return new SimpleContextResult(_previousContext, _callable, output2);
        }

        IContextResult retried = _previousContext.Retry();
        return retried.Failed
            ? new SimpleContextResult(retried, _callable, retried.StripContext())
            : new SimpleContextResult(retried, _callable, _callable());
    }

    public IContextResult Map(Func<Result> mapper) => Failed
        ? new SimpleContextResult(this, mapper, Result.Fail(Error))
        : new SimpleContextResult(this, mapper, mapper());

    public IContextResult<TOut> Map<TOut>(Func<Result<TOut>> mapper) where TOut : notnull => Failed
        ? new IntermediateContextResult<TOut>(Result<TOut>.Fail(Error), mapper, this)
        : new IntermediateContextResult<TOut>(mapper(), mapper, this);

    public IContextResult<TOut> Map<TOut>(Func<TOut> mapper) where TOut : notnull => Map(mapper.WrapInResult());
    public IContextResult Map(Action action) => Map(action.WrapInResult());
}