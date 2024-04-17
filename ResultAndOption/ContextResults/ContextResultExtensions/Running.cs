using Results.CallableGenerators;

namespace Results.ContextResultExtensions;

public static class Running {

    public static IContextResult RunAndGetContext(this Func<Result> action) {
        ICallableGenerator callableGenerator = new SimpleCallableGenerator(action);
        IContextCallable callable = callableGenerator.Generate();
        return new SimpleContextResult(Option<IContextResult>.None(), callable, callable.Call(), callableGenerator);
    }

    public static IContextResult<TOut> RunAndGetContext<TOut>(this Func<Result<TOut>> function) where TOut : notnull {
        ICallableGenerator<TOut> callableGenerator = new CallableGeneratorWithSimpleInput<TOut>(function);
        IContextCallable<TOut> callable = callableGenerator.Generate();
        return new ContextResult<TOut>(callable, Option<IContextResult>.None(), callable.Call(), callableGenerator, new ResultEmitter<TOut>());
    }
}