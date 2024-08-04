namespace ResultAndOption.Results.Getters;

public interface IResultGetter<T> where T : notnull
{
    Result<T> Get();
}