namespace Results.CallableGenerators;

public interface ICallableGenerator {
    IResultCallable Generate();
}

public interface ICallableGenerator<T> where T : notnull {
    IResultCallable<T> Generate();
}