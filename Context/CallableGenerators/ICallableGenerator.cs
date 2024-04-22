using Context.ContextCallables;
using ResultAndOption.ContextCallables;

namespace Context.CallableGenerators;

public interface ICallableGenerator {
    ICallable Generate();
}

public interface ICallableGenerator<T> where T : notnull {
    ICallable<T> Generate();
}