using Results.CallableGenerators;

namespace Results.ContextResultExtensions;

public static class Mapping {
    public static IContextResult Do(this IContextResult context, Action action) {
        ICallableGenerator doGenerator = new SimpleCallableGenerator(action.WrapInResult());
        ICallableGenerator undoGenerator = new SimpleCallableGenerator(Nothing.DoNothingResult);
        ICommandGenerator commandGenerator = new CallableCommandGenerator(doGenerator, undoGenerator);
        return context.Do(commandGenerator);
    }

    public static IContextResult Do(this IContextResult context, Func<Result> action) {
        ICallableGenerator doGenerator = new SimpleCallableGenerator(action);
        ICallableGenerator undoGenerator = new SimpleCallableGenerator(Nothing.DoNothingResult);
        ICommandGenerator commandGenerator = new CallableCommandGenerator(doGenerator, undoGenerator);
        return context.Do(commandGenerator);
    }

    public static IContextResult<T> Do<T>(this IContextResult<T> context, Action action) where T : notnull {
        ICallableGenerator callableGenerator = new SimpleCallableGenerator(action.WrapInResult());
        ICallableGenerator undoGenerator = new SimpleCallableGenerator(Nothing.DoNothingResult);
        ICommandGenerator commandGenerator = new CallableCommandGenerator(callableGenerator, undoGenerator);
        return context.Do(commandGenerator);
    }

    public static IContextResult<T> Do<T>(this IContextResult<T> context, Func<Result> action) where T : notnull {
        ICallableGenerator callableGenerator = new SimpleCallableGenerator(action);
        ICallableGenerator undoGenerator = new SimpleCallableGenerator(Nothing.DoNothingResult);
        ICommandGenerator commandGenerator = new CallableCommandGenerator(callableGenerator, undoGenerator);
        return context.Do(commandGenerator);
    }

    public static IContextResult<T> Do<T>(this IContextResult<T> context, Func<T, Result> action) where T : notnull {
        ResultSubscriber<T> subscriber = new(context.StripContext());
        context.Emitter.Subscribe(subscriber);
        ICallableGenerator callableGenerator = new CallableGeneratorWithSimpleOutput<T>(action, subscriber);
        ICallableGenerator undoGenerator = new SimpleCallableGenerator(Nothing.DoNothingResult);
        ICommandGenerator commandGenerator = new CallableCommandGenerator(callableGenerator, undoGenerator);
        return context.Do(commandGenerator);
    }

    public static IContextResult<T> Do<T>(this IContextResult<T> context, Action<T> action) where T : notnull {
        ResultSubscriber<T> subscriber = new(context.StripContext());
        context.Emitter.Subscribe(subscriber);
        ICallableGenerator callableGenerator = new CallableGeneratorWithSimpleOutput<T>(action.WrapInResult(), subscriber);
        ICallableGenerator undoGenerator = new SimpleCallableGenerator(Nothing.DoNothingResult);
        ICommandGenerator commandGenerator = new CallableCommandGenerator(callableGenerator, undoGenerator);
        return context.Do(commandGenerator);
    }

    public static IContextResult<TOut> Map<TOut>(this IContextResult context, Func<Result<TOut>> mapper) where TOut : notnull {
        ICallableGenerator<TOut> callableGenerator = new CallableGeneratorWithSimpleInput<TOut>(mapper);
        return context.Map(callableGenerator);
    }

    public static IContextResult<TOut> Map<TIn, TOut>(this IContextResult<TIn> context, Func<TIn, Result<TOut>> mapper) where TIn : notnull where TOut : notnull {
        ResultSubscriber<TIn> subscriber = new(context.StripContext());
        context.Emitter.Subscribe(subscriber);
        ICallableGenerator<TOut> callableGenerator = new InputCallableGeneratorOfResult<TIn, TOut>(subscriber, mapper);
        return context.Map(callableGenerator);
    }

    public static IContextResult<TOut> Map<TIn, TOut>(this IContextResult<TIn> context, Func<TIn, TOut> mapper) where TIn : notnull where TOut : notnull {
        ResultSubscriber<TIn> subscriber = new(context.StripContext());
        context.Emitter.Subscribe(subscriber);
        ICallableGenerator<TOut> callableGenerator = new InputCallableGenerator<TIn, TOut>(subscriber, mapper);
        return context.Map(callableGenerator);
    }

    public static IContextResult<TOut> Map<TOut>(this IContextResult context, Func<TOut> mapper) where TOut : notnull {
        ICallableGenerator<TOut> callableGenerator = new CallableGeneratorWithSimpleInput<TOut>(mapper.WrapInResult());
        return context.Map(callableGenerator);
    }
}

internal sealed class SimpleCallableGenerator : ICallableGenerator {
    private readonly Func<Result> _action;

    public SimpleCallableGenerator(Func<Result> action) {
        _action = action;
    }

    public IContextCallable Generate() => new NoInputSimpleContextCallable(_action);
}

internal sealed class CallableGeneratorWithSimpleOutput<TIn> : ICallableGenerator where TIn : notnull {
    private readonly Func<TIn, Result> _action;
    private readonly ResultSubscriber<TIn> _subscriber;

    public CallableGeneratorWithSimpleOutput(Func<TIn, Result> action, ResultSubscriber<TIn> subscriber) {
        _action = action;
        _subscriber = subscriber;
    }

    public IContextCallable Generate() {
        Result<TIn> result = _subscriber.Result;
        return result.Failed
            ? new NoInputSimpleContextCallable(() => Result.Fail(result.Error))
            : new NoOutputContextCallable<TIn>(result.Data, _action);
    }
}