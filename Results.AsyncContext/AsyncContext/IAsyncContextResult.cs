using ResultAndOption.Results;
using Results.AsyncContext.AsyncContext.AsyncCallableGenerators;
using Results.AsyncContext.AsyncContext.AsyncCommandGenerators;

namespace Results.AsyncContext.AsyncContext;

public interface IAsyncContextResult : IResult
{
    Task<IAsyncContextResult> Retry();
    Task Undo();
    IAsyncContextResult<T> Map<T>(IAsyncCallableGenerator<T> asyncCallableGenerator) where T : notnull;
    IAsyncContextResult Do(IAsyncCommandGenerator asyncCommandGenerator);
}

public interface IAsyncContextResult<T> : IAsyncContextResult, IResult<T> where T : notnull
{
    async Task<IAsyncContextResult> IAsyncContextResult.Retry() => await Retry();
    new Task<IAsyncContextResult<T>> Retry();
}