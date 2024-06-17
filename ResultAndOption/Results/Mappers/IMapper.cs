namespace ResultAndOption.Results.Mappers;

public interface IMapper<in TIn, TOut> where TIn : notnull where TOut : notnull
{
    public Result<TOut> Map(TIn data);
}

public interface IMapper<T> where T : notnull
{
    public Result<T> Map();
}