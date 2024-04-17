using System.Runtime.InteropServices.JavaScript;
using Results.CallableGenerators;
using Results.ContextResultExtensions;

namespace Results;

internal class SimpleContextResult : IContextResult {
    private readonly Option<IContextResult> _previousContext;
    private readonly IContextCallable _callable;
    private readonly ICallableGenerator _callableGenerator;
    private readonly Result _result;
    public bool Succeeded => _result.Succeeded;
    public bool Failed => _result.Failed;
    public IError Error => _result.Error;

    public SimpleContextResult(Option<IContextResult> previousContext, IContextCallable callable, Result result, ICallableGenerator callableGenerator) {
        _previousContext = previousContext;
        _callable = callable;
        _result = result;
        _callableGenerator = callableGenerator;
    }

    public Result StripContext() => _result;
    IContextResult IContextResult.Retry() => Retry();

    public SimpleContextResult Retry() {
        if (Succeeded) return this;
        if (_previousContext.IsNone()) return new SimpleContextResult(Option<IContextResult>.None(), _callable, _callable.Call(), _callableGenerator);
        IContextResult retried = _previousContext.Data.Retry();
        return retried.Failed
            ? new SimpleContextResult(_previousContext, _callable, Result.Fail(retried.Error), _callableGenerator)
            : new SimpleContextResult(_previousContext, _callable, _callable.Call(), _callableGenerator);
    }

    public IContextResult Do(Func<Result> action) {
        ICallableGenerator callableGenerator = new SimpleCallableGenerator(action);
        IContextCallable callable = callableGenerator.Generate();
        return Failed
            ? new SimpleContextResult(this.ToOption<IContextResult>(), callable, Result.Fail(Error), callableGenerator)
            : new SimpleContextResult(this.ToOption<IContextResult>(), callable, callable.Call(), callableGenerator);
    }

    public IContextResult<TOut> Map<TOut>(Func<Result<TOut>> mapper) where TOut : notnull {
        ICallableGenerator<TOut> callableGenerator = new CallableGeneratorWithSimpleInput<TOut>(mapper);
        IContextCallable<TOut> callable = callableGenerator.Generate();
        return Failed
            ? new ContextResult<TOut>(callable, this.ToOption<IContextResult>(), Result<TOut>.Fail(Error), callableGenerator, new ResultEmitter<TOut>())
            : new ContextResult<TOut>(callable, this.ToOption<IContextResult>(), callable.Call(), callableGenerator, new ResultEmitter<TOut>());
    }

    public IContextResult<TOut> Map<TOut>(Func<TOut> mapper) where TOut : notnull => Map(mapper.WrapInResult());
    public IContextResult Do(Action action) => Do(action.WrapInResult());
}