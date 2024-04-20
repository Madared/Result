using Results.CallableGenerators;
using Results.ContextResultExtensions;

namespace Results;

internal sealed class ContextResult<TOut> : IContextResult<TOut> where TOut : notnull {
    private readonly ICallableGenerator<TOut> _callableGenerator;
    private readonly IContextCallable<TOut> _called;
    private readonly Option<IContextResult> _previousContext;
    private readonly Result<TOut> _result;

    public ContextResult(IContextCallable<TOut> called, Option<IContextResult> previousContext, Result<TOut> result, ICallableGenerator<TOut> callableGenerator, ResultEmitter<TOut> emitter) {
        _called = called;
        _previousContext = previousContext;
        _result = result;
        Emitter = emitter;
        _callableGenerator = callableGenerator;
    }

    public ResultEmitter<TOut> Emitter { get; }
    public IError Error => _result.Error;
    public TOut Data => _result.Data;
    public bool Succeeded => _result.Succeeded;
    public bool Failed => _result.Failed;


    public Result<TOut> StripContext() {
        return _result;
    }

    public void Undo() {
        if (_previousContext.IsSome()) _previousContext.Data.Undo();
    }

    public IContextResult<TOut> Do(ICommandGenerator commandGenerator) {
        var command = commandGenerator.Generate();
        ResultSubscriber<TOut> subscriber = new(_result);
        Emitter.Subscribe(subscriber);
        ICallableGenerator<TOut> callableGenerator = new ResultGetterCallableGenerator<TOut>(subscriber);
        IContextResult simpleContext = Failed
            ? new SimpleContextResult(this.ToOption<IContextResult>(), command, commandGenerator, Result.Fail(Error))
            : new SimpleContextResult(this.ToOption<IContextResult>(), command, commandGenerator, command.Call());
        return simpleContext.Map(callableGenerator);
    }

    public IContextResult<TNext> Map<TNext>(ICallableGenerator<TNext> callableGenerator) where TNext : notnull {
        IContextCallable<TNext> callable = callableGenerator.Generate();
        return Failed
            ? new ContextResult<TNext>(callable, this.ToOption<IContextResult>(), Result<TNext>.Fail(Error), callableGenerator, new ResultEmitter<TNext>())
            : new ContextResult<TNext>(callable, this.ToOption<IContextResult>(), callable.Call(), callableGenerator, new ResultEmitter<TNext>());
    }

    public IContextResult<TOut> Retry() {
        if (Succeeded) return this;
        if (_previousContext.IsNone()) return new ContextResult<TOut>(_called, Option<IContextResult>.None(), _called.Call(), _callableGenerator, new ResultEmitter<TOut>());

        var retried = _previousContext.Data.Retry();
        IContextCallable<TOut> updatedCalled = _callableGenerator.Generate();
        if (retried.Failed) return new ContextResult<TOut>(updatedCalled, retried.ToOption(), Result<TOut>.Fail(retried.Error), _callableGenerator, new ResultEmitter<TOut>());

        Result<TOut> updatedResult = updatedCalled.Call();
        Emitter.Emit(updatedResult);
        return new ContextResult<TOut>(updatedCalled, retried.ToOption(), updatedResult, _callableGenerator, new ResultEmitter<TOut>());
    }
}