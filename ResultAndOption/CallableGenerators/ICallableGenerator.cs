namespace Results.CallableGenerators;

public interface ICallableGenerator {
    IContextCallable Generate();
}

public interface ICallableGenerator<T> where T : notnull {
    IContextCallable<T> Generate();
}