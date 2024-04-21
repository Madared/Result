namespace Results;

public interface IResultCallable {
    Result Call();
}

public interface IResultCallable<TOut> : IResultCallable where TOut : notnull {
    Result IResultCallable.Call() {
        return Call().ToSimpleResult();
    }

    Result<TOut> Call();
}