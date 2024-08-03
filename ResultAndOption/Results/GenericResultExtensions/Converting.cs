using ResultAndOption.Errors;
using ResultAndOption.Options;

namespace ResultAndOption.Results.GenericResultExtensions;

/// <summary>
/// Contains Conversion methods for Generic Results
/// </summary>
public static class Converting
{
    /// <summary>
    ///     Converts a nullable reference to a result, representing a success or failure state.
    /// </summary>
    /// <typeparam name="TIn">The type of the nullable input reference.</typeparam>
    /// <param name="i">The nullable input reference.</param>
    /// <param name="error">The error to use if the input reference is null.</param>
    /// <returns>A result representing the input reference if it is not null, or a failed result with the specified error.</returns>
    public static Result<TIn> ToResult<TIn>(this TIn? i, IError error) where TIn : notnull => Result<TIn>.Unknown(i, error);
    
    /// <summary>
    ///     Converts a simple list of results to the more specific ResultList.
    /// </summary>
    /// <param name="results">The list of results to convert</param>
    /// <typeparam name="TIn">The type of data carried by the result</typeparam>
    /// <returns>A ResultList</returns>
    public static ResultList<TIn> ToResultList<TIn>(this IEnumerable<Result<TIn>> results)
        where TIn : notnull
    {
        ResultList<TIn> resultList = new();
        resultList.AddResults(results);
        return resultList;
    }
    
    /// <summary>
    /// Turns an option to a result, an empty option returns a failed result with the specified IError
    /// </summary>
    /// <param name="data"></param>
    /// <param name="error"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Result<T> ToResult<T>(this Option<T> data, IError error) where T : notnull => data.IsNone()
        ? Result<T>.Fail(error)
        : Result<T>.Ok(data.Data);
}