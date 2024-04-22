using ResultAndOption.ContextCallables;
using Results.Context.ContextCallables;

namespace Results.Context.CallableGenerators;

public interface ICallableGenerator {
    ICallable Generate();
}

public interface ICallableGenerator<T> where T : notnull {
    ICallable<T> Generate();
}