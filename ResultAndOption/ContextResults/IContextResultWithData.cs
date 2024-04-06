namespace Results;

public interface IContextResult : IResult {
    IContextResult Retry();
}

public interface IContextResultWithData<TOut> : IContextResult, IResultWithData<TOut> where TOut : notnull {
    IContextResult IContextResult.Retry() => Retry();
    IContextResultWithData<TOut> Retry();
    Result<TOut> StripContext();
}