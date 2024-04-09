namespace Results;

public interface IContextResult : IResult {
    IContextResult Retry();
    Result StripContext();

    
    IContextResult Map(Action action);
    IContextResult Map(Func<Result> mapper);
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
    IContextResult Map(Func<TOut, Result> mapper);
    IContextResult Map(Action<TOut> mapper);
}