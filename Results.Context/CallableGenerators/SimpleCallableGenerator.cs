using ResultAndOption.Results;
using Results.Context.ContextCallables;

namespace Results.Context.CallableGenerators;

internal sealed class SimpleCallableGenerator : ICallableGenerator {
    private readonly Func<Result> _action;

    public SimpleCallableGenerator(Func<Result> action) {
        _action = action;
    }

    public ICallable Generate() {
        return new NoInputSimpleCallable(_action);
    }
}