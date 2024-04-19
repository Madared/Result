using Results.ActionCallables;
using Results.CallableGenerators;

namespace Results.ContextResultExtensions;

public static class Running {
    public static IContextResult RunAndGetContext(this Func<Result> action) {
        ICallableGenerator doGenerator = new SimpleCallableGenerator(action);
        IActionCallableGenerator undoGenerator = new ActionCallableGenerator(Nothing.DoNothing);
        ICommandGenerator commandGenerator = new CallableCommandGenerator(doGenerator, undoGenerator);
        var command = commandGenerator.Generate();
        return new SimpleContextResult(Option<IContextResult>.None(), command, commandGenerator, command.Call());
    }

    public static IContextResult<TOut> RunAndGetContext<TOut>(this Func<Result<TOut>> function) where TOut : notnull {
        ICallableGenerator<TOut> callableGenerator = new CallableGeneratorWithSimpleInput<TOut>(function);
        IContextCallable<TOut> callable = callableGenerator.Generate();
        return new ContextResult<TOut>(callable, Option<IContextResult>.None(), callable.Call(), callableGenerator, new ResultEmitter<TOut>());
    }
}

internal sealed class CallableGeneratorWithSimpleInput<T> : ICallableGenerator<T> where T : notnull {
    private readonly Func<Result<T>> _action;

    public CallableGeneratorWithSimpleInput(Func<Result<T>> action) {
        _action = action;
    }

    public IContextCallable<T> Generate() {
        return new CallableWithSimpleInput<T>(_action);
    }
}

internal sealed class CallableWithSimpleInput<T> : IContextCallable<T> where T : notnull {
    private readonly Func<Result<T>> _action;

    public CallableWithSimpleInput(Func<Result<T>> action) {
        _action = action;
    }

    public Result<T> Call() {
        return _action();
    }
}