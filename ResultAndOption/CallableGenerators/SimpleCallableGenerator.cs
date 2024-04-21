using Results.CallableGenerators;

namespace Results.ContextResultExtensions;

internal sealed class SimpleCallableGenerator : ICallableGenerator {
    private readonly Func<Result> _action;

    public SimpleCallableGenerator(Func<Result> action) {
        _action = action;
    }

    public IResultCallable Generate() {
        return new NoInputSimpleResultCallable(_action);
    }
}