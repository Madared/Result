using Results.CallableGenerators;
using Results.ContextResultExtensions;

namespace Results;

internal class ContextResult<TOut> : IContextResult<TOut> where TOut : notnull {
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
        return Failed
            ? new ContextResult<TNext>(callable, this.ToOption<IContextResult>(), Result<TNext>.Fail(Error), callableGenerator, new ResultEmitter<TNext>())
            : new ContextResult<TNext>(callable, this.ToOption<IContextResult>(), callable.Call(), callableGenerator, new ResultEmitter<TNext>());
    }

    public IContextResult<TOut> Do(Func<TOut, Result> mapper) {
        ResultSubscriber<TOut> subscriber = new(_result);
        _resultEmitter.Subscribe(subscriber);
        ICallableGenerator simpleCallableGenerator = new CallableGeneratorWithSimpleOutput<TOut>(subscriber, mapper);
        IContextCallable simpleCallable = simpleCallableGenerator.Generate();
        ICallableGenerator<TOut> callableGenerator = new CurrentResultActionCallableGenerator<TOut>(subscriber, simpleCallable);
        IContextCallable<TOut> callable = callableGenerator.Generate();
        return Failed
            ? new ContextResult<TOut>(callable, this.ToOption<IContextResult>(), Result<TOut>.Fail(Error), callableGenerator, new ResultEmitter<TOut>())
            : new ContextResult<TOut>(callable, this.ToOption<IContextResult>(), callable.Call(), callableGenerator, new ResultEmitter<TOut>());
    }

    public IContextResult<TOut> Do(Func<Result> action) {
        ResultSubscriber<TOut> subscriber = new(_result);
        _resultEmitter.Subscribe(subscriber);
        ICallableGenerator simpleCallableGenerator = new SimpleCallableGenerator(action);
        IContextCallable simpleCallable = simpleCallableGenerator.Generate();
        ICallableGenerator<TOut> callableGenerator = new CurrentResultActionCallableGenerator<TOut>(subscriber, simpleCallable);
        IContextCallable<TOut> callable = callableGenerator.Generate();
        return Failed
            ? new ContextResult<TOut>(callable, this.ToOption<IContextResult>(), Result<TOut>.Fail(Error), callableGenerator, new ResultEmitter<TOut>())
            : new ContextResult<TOut>(callable, this.ToOption<IContextResult>(), callable.Call(), callableGenerator, new ResultEmitter<TOut>());
    }

    public IContextResult<TOut> Do(Action<TOut> action) {
        Func<TOut, Result> generated = action.WrapInResult();
        ResultSubscriber<TOut> subscriber = new(_result);
        _resultEmitter.Subscribe(subscriber);
        ICallableGenerator simpleCallableGenerator = new CallableGeneratorWithSimpleOutput<TOut>(subscriber, generated);
        IContextCallable simpleCallable = simpleCallableGenerator.Generate();
        ICallableGenerator<TOut> callableGenerator = new CurrentResultActionCallableGenerator<TOut>(subscriber, simpleCallable);
        IContextCallable<TOut> callable = callableGenerator.Generate();
        return Failed
            ? new ContextResult<TOut>(callable, this.ToOption<IContextResult>(), Result<TOut>.Fail(Error), callableGenerator, new ResultEmitter<TOut>())
            : new ContextResult<TOut>(callable, this.ToOption<IContextResult>(), callable.Call(), callableGenerator, new ResultEmitter<TOut>());
    }

    public IContextResult<TNext> Map<TNext>(Func<Result<TNext>> mapper) where TNext : notnull {
        ICallableGenerator<TNext> callableGenerator = new CallableGeneratorWithSimpleInput<TNext>(mapper);
        IContextCallable<TNext> callable = callableGenerator.Generate();
        return Failed
            ? new ContextResult<TNext>(callable, this.ToOption<IContextResult>(), Result<TNext>.Fail(Error), callableGenerator, new ResultEmitter<TNext>())
            : new ContextResult<TNext>(callable, this.ToOption<IContextResult>(), callable.Call(), callableGenerator, new ResultEmitter<TNext>());
    }

    public IContextResult<TNext> Map<TNext>(Func<TNext> mapper) where TNext : notnull => Map(mapper.WrapInResult());
    public IContextResult<TOut> Do(Action action) => Do(action.WrapInResult());
    public IContextResult Map(Action<TOut> mapper) => Do(mapper.WrapInResult());
    public IContextResult<TNext> Map<TNext>(Func<TOut, TNext> mapper) where TNext : notnull => Map(mapper.WrapInResult());
}

internal sealed class CurrentResultActionCallableGenerator<TOut> : ICallableGenerator<TOut> where TOut : notnull {
    private readonly ResultSubscriber<TOut> _subscriber;
    private readonly IContextCallable _callable;

    public CurrentResultActionCallableGenerator(ResultSubscriber<TOut> subscriber, IContextCallable callable) {
        _subscriber = subscriber;
        _callable = callable;
    }

    public IContextCallable<TOut> Generate() => new CurrentResultActionCallable<TOut>(_subscriber.Result, _callable);
}

internal sealed class CurrentResultActionCallable<TOut> : IContextCallable<TOut> where TOut : notnull {
    private Result<TOut> _result;
    private IContextCallable _callable;

    public CurrentResultActionCallable(Result<TOut> result, IContextCallable callable) {
        _result = result;
        _callable = callable;
    }

    public Result<TOut> Call() {
        Result called = _callable.Call();
        return called.Failed ? Result<TOut>.Fail(called.Error) : _result;
    }
}