namespace Results;

public interface IContextCallable {
    Result Call();
}

public interface IContextCallable<TOut> : IContextCallable where TOut : notnull {
    Result IContextCallable.Call() {
        return Call().ToSimpleResult();
    }

    Result<TOut> Call();
}