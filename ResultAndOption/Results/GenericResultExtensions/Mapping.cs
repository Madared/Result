namespace ResultAndOption.Results.GenericResultExtensions;

public static class Mapping {
    /// <summary>
    ///     Maps the data of the result using the specified mapper function and wrapps it into a result of the new type.
    /// </summary>
    /// <typeparam name="TResult">The type of data to map to.</typeparam>
    /// <typeparam name="T"></typeparam>
    /// <param name="result"></param>
    /// <param name="mapper">The mapper function.</param>
    /// <returns>A new result with the mapped data.</returns>
    public static Result<TResult> Map<T, TResult>(this Result<T> result, Func<T, TResult> mapper) where T : notnull where TResult : notnull => result.Failed
        ? Result<TResult>.Fail(result.Error)
        : Result<TResult>.Ok(mapper(result.Data));

    /// <summary>
    ///     Pipes the data into a function that also returns a result or passes the error to the new result.
    /// </summary>
    /// <typeparam name="TResult">The type of data to map to.</typeparam>
    /// <typeparam name="T"></typeparam>
    /// <param name="result"></param>
    /// <param name="mapper">The mapper function.</param>
    /// <returns>A new result with the mapped data.</returns>
    public static Result<TResult> Map<T, TResult>(this Result<T> result, Func<T, Result<TResult>> mapper) where T : notnull where TResult : notnull => result.Failed
        ? Result<TResult>.Fail(result.Error)
        : mapper(result.Data);

    /// <summary>
    ///     Returns the internal data in case of success or the replacement value passed in
    /// </summary>
    /// <param name="result"></param>
    /// <param name="data">Replacement value to use in case of failed result</param>
    /// <returns></returns>
    public static T Or<T>(this Result<T> result, T data) where T : notnull => result.Failed ? data : result.Data;
}