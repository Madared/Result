using Results.CallableGenerators;
using Results.ContextResultExtensions;

namespace Results;

internal sealed class ContextResult<TOut> : IContextResult<TOut> where TOut : notnull {
    private readonly Option<IContextResult> _previousContext;
    private readonly ICallableGenerator<TOut> _callableGenerator;
    private readonly ResultEmitter<TOut> _resultEmitter;
    private readonly IContextCallable<TOut> _called;
    private readonly Result<TOut> _result;


    public IError Error => _result.Error;
    public TOut Data => _result.Data;
    public bool Succeeded => _result.Succeeded;
    public bool Failed => _result.Failed;

    public ContextResult(IContextCallable<TOut> called, Option<IContextResult> previousContext, Result<TOut> result, ICallableGenerator<TOut> callableGenerator, ResultEmitter<TOut> resultEmitter) {
        _called = called;
        _previousContext = previousContext;
        _result = result;
        _resultEmitter = resultEmitter;
        _callableGenerator = callableGenerator;
    }

    IContextResult<TOut> IContextResult<TOut>.Retry() => Retry();

    public Result<TOut> StripContext() => _result;

    public ContextResult<TOut> Retry() {
        if (Succeeded) return this;
        if (_previousContext.IsNone()) {
            return new ContextResult<TOut>(_called, Option<IContextResult>.None(), _called.Call(), _callableGenerator, _resultEmitter);
        }

        IContextResult retried = _previousContext.Data.Retry();
        IContextCallable<TOut> updatedCalled = _callableGenerator.Generate();
        if (retried.Failed) {
            return new ContextResult<TOut>(updatedCalled, retried.ToOption(), Result<TOut>.Fail(retried.Error), _callableGenerator, _resultEmitter);
        }

        Result<TOut> updatedResult = updatedCalled.Call();
        _resultEmitter.Emit(updatedResult);
        return new ContextResult<TOut>(updatedCalled, retried.ToOption(), updatedResult, _callableGenerator, _resultEmitter);
    }


    public IContextResult<TNext> Map<TNext>(Func<TOut, Result<TNext>> mapper) where TNext : notnull {
        ResultSubscriber<TOut> subscriber = new(_result);
        _resultEmitter.Subscribe(subscriber);
        ICallableGenerator<TNext> callableGenerator = new CallableGenerator<TOut, TNext>(subscriber, mapper);
        IContextCallable<TNext> callable = callableGenerator.Generate();
        return Create(callable, callableGenerator);
    }

    public IContextResult<TOut> Do(Func<TOut, Result> mapper) {
        ResultSubscriber<TOut> subscriber = new(_result);
        _resultEmitter.Subscribe(subscriber);
        ICallableGenerator simpleCallableGenerator = new CallableGeneratorWithSimpleOutput<TOut>(subscriber, mapper);
        IContextCallable simpleCallable = simpleCallableGenerator.Generate();
        ICallableGenerator<TOut> callableGenerator = new CurrentResultActionCallableGenerator<TOut>(subscriber, simpleCallable);
        IContextCallable<TOut> callable = callableGenerator.Generate();
        return Create(callable, callableGenerator);
    }

    public IContextResult<TOut> Do(Func<Result> action) {
        ResultSubscriber<TOut> subscriber = new(_result);
        _resultEmitter.Subscribe(subscriber);
        ICallableGenerator simpleCallableGenerator = new SimpleCallableGenerator(action);
        IContextCallable simpleCallable = simpleCallableGenerator.Generate();
        ICallableGenerator<TOut> callableGenerator = new CurrentResultActionCallableGenerator<TOut>(subscriber, simpleCallable);
        IContextCallable<TOut> callable = callableGenerator.Generate();
        return Create(callable, callableGenerator);
    }

    public IContextResult<TOut> Do(Action<TOut> action) {
        Func<TOut, Result> generated = action.WrapInResult();
        ResultSubscriber<TOut> subscriber = new(_result);
        _resultEmitter.Subscribe(subscriber);
        ICallableGenerator simpleCallableGenerator = new CallableGeneratorWithSimpleOutput<TOut>(subscriber, generated);
        IContextCallable simpleCallable = simpleCallableGenerator.Generate();
        ICallableGenerator<TOut> callableGenerator = new CurrentResultActionCallableGenerator<TOut>(subscriber, simpleCallable);
        IContextCallable<TOut> callable = callableGenerator.Generate();
        return Create(callable, callableGenerator);
    }

    public IContextResult<TNext> Map<TNext>(Func<Result<TNext>> mapper) where TNext : notnull {
        ICallableGenerator<TNext> callableGenerator = new CallableGeneratorWithSimpleInput<TNext>(mapper);
        IContextCallable<TNext> callable = callableGenerator.Generate();
        return Create(callable, callableGenerator);
    }

    public IContextResult<TNext> Map<TNext>(Func<TNext> mapper) where TNext : notnull => Map(mapper.WrapInResult());
    public IContextResult<TOut> Do(Action action) => Do(action.WrapInResult());
    public IContextResult Map(Action<TOut> mapper) => Do(mapper.WrapInResult());
    public IContextResult<TNext> Map<TNext>(Func<TOut, TNext> mapper) where TNext : notnull => Map(mapper.WrapInResult());

    private IContextResult<TNext> Create<TNext>(IContextCallable<TNext> callable, ICallableGenerator<TNext> callableGenerator) where TNext : notnull => Failed
        ? new ContextResult<TNext>(callable, this.ToOption<IContextResult>(), Result<TNext>.Fail(Error), callableGenerator, new ResultEmitter<TNext>())
        : new ContextResult<TNext>(callable, this.ToOption<IContextResult>(), callable.Call(), callableGenerator, new ResultEmitter<TNext>());
}