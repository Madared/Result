namespace ResultAndOption.Results.Getters;

public interface IAsyncGetter<T> where T : notnull
{
    Task<T> Get(CancellationToken? token = null);
}