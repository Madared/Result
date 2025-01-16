using ResultAndOption.Errors;
using ResultAndOption.Options;

namespace ResultAndOption.Results.GenericResultExtensions;

/// <summary>
/// Contains Conversion methods for Generic Results
/// </summary>
public static class Converting
{
    /// <summary>
    ///     Converts the result to a simple result without carrying any data.
    /// </summary>
    /// <returns>A simple result representing the success or failure of the original result.</returns>
    public static Result ToSimpleResult<T>(this in Result<T> result) where T : notnull => result.Failed
        ? Result.Fail(result.CustomError)
        : Result.Ok();

    /// <summary>
    ///     Converts the result to a result with a different data type, assuming the original result represents an error.
    /// </summary>
    /// <typeparam name="TResult">The type of output result.</typeparam>
    /// <typeparam name="T">The type of the input result</typeparam>
    /// <returns>A result with the specified data type if the original result represents a success.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the original result represents a success.</exception>
    public static Result<TResult> ConvertErrorResult<T, TResult>(this in Result<T> result) where T : notnull where TResult : notnull => result.Failed
        ? Result<TResult>.Fail(result.CustomError)
        : throw new InvalidOperationException(
            "Cannot convert error result when the original result represents a success.");
    
    /// <summary>
    ///     Converts a nullable reference to a result, representing a success or failure state.
    /// </summary>
    /// <typeparam name="TIn">The type of the nullable input reference.</typeparam>
    /// <param name="i">The nullable input reference.</param>
    /// <param name="customError">The error to use if the input reference is null.</param>
    /// <returns>A result representing the input reference if it is not null, or a failed result with the specified error.</returns>
    public static Result<TIn> ToResult<TIn>(this TIn? i, CustomError customError) where TIn : notnull => Result<TIn>.Unknown(i, customError);
    
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
    /// <param name="customError"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Result<T> ToResult<T>(this Option<T> data, CustomError customError) where T : notnull => data.IsNone()
        ? Result<T>.Fail(customError)
        : Result<T>.Ok(data.Data);
}