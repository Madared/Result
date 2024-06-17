namespace ResultAndOption.Results.Mappers;

public interface IAsyncMapper<T> where T : notnull
{
    public Task<Result<T>> Map();
}

public interface IAsyncMapper<in TIn, TOut> where TIn : notnull where TOut : notnull
{
    public Task<Result<TOut>> Map(TIn data);
}