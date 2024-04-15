namespace Results;

public interface IMappableContext {
    IContextResult Map(Action action);
    IContextResult Map(Func<Result> mapper);
    IContextResult<TOut> Map<TOut>(Func<Result<TOut>> mapper) where TOut : notnull;
    IContextResult<TOut> Map<TOut>(Func<TOut> mapper) where TOut : notnull;
}

public interface IMappableContext<out TOut> {
    IContextResult<TNext> Map<TNext>(Func<TOut, Result<TNext>> mapper) where TNext : notnull;
    IContextResult<TNext> Map<TNext>(Func<TOut, TNext> mapper) where TNext : notnull;
    IContextResult Map(Func<TOut, Result> mapper);
    IContextResult Map(Action<TOut> mapper);
}

public interface IStrippableContext {
    Result StripContext();
}

public interface IStrippableContext<TOut> where TOut : notnull {
    Result<TOut> StripContext();
}

public interface IRetryableContext {
    IContextResult Retry();
}

public interface IRetryableContext<TOut> where TOut : notnull {
    IContextResult<TOut> Retry();
}


public interface IContextResult : IResult {
    IContextResult Retry();
    Result StripContext();


    IContextResult Do(Action action);
    IContextResult Do(Func<Result> action);
    IContextResult<TOut> Map<TOut>(Func<Result<TOut>> mapper) where TOut : notnull;
    IContextResult<TOut> Map<TOut>(Func<TOut> mapper) where TOut : notnull;
}

public interface IContextResult<TOut> : IContextResult, IResult<TOut> where TOut : notnull {
    IContextResult IContextResult.Retry() => Retry();
    IContextResult<TOut> Retry();

    Result IContextResult.StripContext() => StripContext().ToSimpleResult();
    Result<TOut> StripContext();

    IContextResult<TNext> Map<TNext>(Func<TOut, Result<TNext>> mapper) where TNext : notnull;
    IContextResult<TNext> Map<TNext>(Func<TOut, TNext> mapper) where TNext : notnull;
    IContextResult<TOut> Do(Func<TOut, Result> mapper);

    IContextResult IContextResult.Do(Action action) => Do(action);
    IContextResult IContextResult.Do(Func<Result> action) => Do(action);

    IContextResult<TOut> Do(Action action);
    IContextResult<TOut> Do(Action<TOut> action);
    IContextResult<TOut> Do(Func<Result> action);
}