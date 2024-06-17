namespace ResultAndOption.Results.Mappers;

/// <summary>
/// A Synchronous Mapper with input
/// </summary>
/// <typeparam name="TIn"></typeparam>
/// <typeparam name="TOut"></typeparam>
public interface IMapper<in TIn, TOut> where TIn : notnull where TOut : notnull
{
    /// <summary>
    /// Mapper call with input
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public Result<TOut> Map(TIn data);
}

/// <summary>
/// A synchronous Mapper
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IMapper<T> where T : notnull
{
    /// <summary>
    /// Mapper call
    /// </summary>
    /// <returns></returns>
    public Result<T> Map();
}