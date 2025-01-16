using ResultAndOption.Results;

namespace ResultAndOption.Results.Mappers;

/// <summary>
/// An Asynchronous Mapper
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IAsyncMapper<T> where T : notnull
{
    /// <summary>
    /// Asynchronous Mapper call
    /// </summary>
    /// <returns></returns>
    public Task<Result<T>> Map(CancellationToken? token = null);
}

/// <summary>
/// An Asynchronous Mapper with input
/// </summary>
/// <typeparam name="TIn">The input type</typeparam>
/// <typeparam name="TOut">The output type</typeparam>
public interface IAsyncMapper<in TIn, TOut> where TIn : notnull where TOut : notnull
{
    /// <summary>
    /// Asynchronous Mapper call with input
    /// </summary>
    /// <param name="data">The data to map</param>
    /// <param name="token">The cancellation token</param>
    /// <returns></returns>
    public Task<Result<TOut>> Map(TIn data, CancellationToken? token = null);
}