using ResultAndOption.Results.Mappers;

namespace ResultAndOption.Results.GenericResultExtensions;

public static class Mapping
{
    /// <summary>
    /// Uses an IMapper to map a successful result data through its transformation by taking the data as the only parameter
    /// or into a new failed result with the existing error.
    /// </summary>
    /// <param name="mapper"></param>
    /// <typeparam name="TOut"></typeparam>
    /// <returns>A new result</returns>
    public static Result<TOut> Map<TIn, TOut>(this in Result<TIn> result, IMapper<TIn, TOut> mapper)
        where TIn : notnull where TOut : notnull => result.Failed
        ? Result<TOut>.Fail(result.Error)
        : mapper.Map(result.Data);




    /// <summary>
    /// Maps the data of the result using the specified mapper function and wrapps it into a result of the new type.
    /// </summary>
    /// <typeparam name="TResult">The type of data to map to.</typeparam>
    /// <typeparam name="T"></typeparam>
    /// <param name="mapper">The mapper function.</param>
    /// <returns>A new result with the mapped data.</returns>
    public static Result<TResult> Map<TIn, TResult>(this in Result<TIn> result, Func<TIn, TResult> mapper)
        where TIn : notnull where TResult : notnull => result.Failed
        ? Result<TResult>.Fail(result.Error)
        : Result<TResult>.Ok(mapper(result.Data));

    /// <summary>
    ///     Pipes the data into a function that also returns a result or passes the error to the new result.
    /// </summary>
    /// <typeparam name="TResult">The type of data to map to.</typeparam>
    /// <typeparam name="T"></typeparam>
    /// <param name="mapper">The mapper function.</param>
    /// <returns>A new result with the mapped data.</returns>
    public static Result<TResult> Map<TIn, TResult>(this in Result<TIn> result, Func<TIn, Result<TResult>> mapper)
        where TIn : notnull where TResult : notnull => result.Failed
        ? Result<TResult>.Fail(result.Error)
        : mapper(result.Data);
    
    /// <summary>
    ///     Returns the internal data in case of success or the replacement value passed in
    /// </summary>
    /// <param name="data">Replacement value to use in case of failed result</param>
    /// <returns></returns>
    public static T Or<T>(this in Result<T> result, T data) where T : notnull => result.Failed ? data : result.Data;
}