namespace Results.CallableGenerators;

public interface ICallableGenerator {
    ICallable Generate();
}

public interface ICallableGenerator<T> where T : notnull {
    ICallable<T> Generate();
}