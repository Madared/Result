namespace Results.CallableGenerators;

public class CallableGeneratorWithSimpleInput<TOut> : ICallableGenerator<TOut> where TOut : notnull {
    private readonly Func<Result<TOut>> _func;
    public IContextCallable<TOut> Generate() => new NoInputContextCallable<TOut>(_func);
}