namespace Results.CallableGenerators;

public class SimpleCallableGenerator : ICallableGenerator {
    private readonly Func<Result> _func;
    public SimpleCallableGenerator(Func<Result> func) {
        _func = func;
    }
    public IContextCallable Generate() => new NoInputSimpleContextCallable(_func);
}