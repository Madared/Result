using ResultAndOption.CallableGenerators;
using ResultAndOption.ContextCallables;
using ResultAndOption.Results;

namespace ResultAndOption.ContextResultExtensions;

internal sealed class SimpleCallableGenerator : ICallableGenerator {
    private readonly Func<Result> _action;

    public SimpleCallableGenerator(Func<Result> action) {
        _action = action;
    }

    public ICallable Generate() {
        return new NoInputSimpleCallable(_action);
    }
}