namespace Results.CallableGenerators;

public interface ICallableGenerator<TOut> where TOut : notnull {
    
    IContextCallable<TOut> Generate();
}
public interface ICallableGenerator {
    IContextCallable Generate();
}