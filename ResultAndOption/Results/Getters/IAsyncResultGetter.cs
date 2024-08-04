namespace ResultAndOption.Results.Getters;

public interface IAsyncResultGetter<T> where T : notnull
{
    Task<Result<T>> Get(CancellationToken? token = null);
}