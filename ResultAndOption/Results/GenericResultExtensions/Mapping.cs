using ResultAndOption.Results.Mappers;

namespace ResultAndOption.Results.GenericResultExtensions;

/// <summary>
/// Extension methods for mapping generic results
/// </summary>
public static class Mapping
{
    /// <summary>
    /// Uses an IMapper to map a successful result data through its transformation by taking the data as the only parameter
    /// or into a new failed result with the existing error.
    /// </summary>
    /// <param name="result">The result to check</param>
    /// <param name="mapper">The mapper to run</param>
    /// <typeparam name="TOut">The type of the output result</typeparam>
    /// <typeparam name="TIn">The type of the input result</typeparam>
    /// <returns>A new result</returns>
    public static Result<TOut> Map<TIn, TOut>(this in Result<TIn> result, IMapper<TIn, TOut> mapper)
        where TIn : notnull where TOut : notnull => result.Failed
        ? Result<TOut>.Fail(result.CustomError)
        : mapper.Map(result.Data);

    /// <summary>
    /// Maps the data of the result using the specified mapper function and wrapps it into a result of the new type.
    /// </summary>
    /// <typeparam name="TResult">The type of data to map to.</typeparam>
    /// <typeparam name="TIn">The type of the input result</typeparam>
    /// <param name="result">The result to check</param>
    /// <param name="mapper">The mapper function.</param>
    /// <returns>A new result with the mapped data.</returns>
    public static Result<TResult> Map<TIn, TResult>(this in Result<TIn> result, Func<TIn, TResult> mapper)
        where TIn : notnull where TResult : notnull => result.Failed
        ? Result<TResult>.Fail(result.CustomError)
        : Result<TResult>.Ok(mapper(result.Data));

    /// <summary>
    ///     Pipes the data into a function that also returns a result or passes the error to the new result.
    /// </summary>
    /// <typeparam name="TResult">The type of data to map to.</typeparam>
    /// <typeparam name="TIn">The type of the input result</typeparam>
    /// <param name="result">the result to map</param>
    /// <param name="mapper">The mapper function.</param>
    /// <returns>A new result with the mapped data.</returns>
    public static Result<TResult> Map<TIn, TResult>(this in Result<TIn> result, Func<TIn, Result<TResult>> mapper)
        where TIn : notnull where TResult : notnull => result.Failed
        ? Result<TResult>.Fail(result.CustomError)
        : mapper(result.Data);

    /// <summary>
    ///     Returns the internal data in case of success or the replacement value passed in
    /// </summary>
    /// <param name="result">The result to check</param>
    /// <param name="data">Replacement value to use in case of failed result</param>
    /// <returns></returns>
    public static T Or<T>(this in Result<T> result, T data) where T : notnull => result.Failed ? data : result.Data;
}
