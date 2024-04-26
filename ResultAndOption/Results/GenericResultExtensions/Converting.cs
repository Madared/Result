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
    public static Result ToSimpleResult<T>(this Result<T> result) where T : notnull => result.Failed
        ? Result.Fail(result.Error)
        : Result.Ok();

    /// <summary>
    ///     Converts the result to a result with a different data type, assuming the original result represents an error.
    /// </summary>
    /// <typeparam name="TResult">The type of data carried by the new result.</typeparam>
    /// <typeparam name="T"></typeparam>
    /// <returns>A result with the specified data type if the original result represents a success.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the original result represents a failure.</exception>
    public static Result<TResult> ConvertErrorResult<T, TResult>(this Result<T> result)
        where T : notnull where TResult : notnull => result.Failed
        ? Result<TResult>.Fail(result.Error)
        : throw new InvalidOperationException(
            "Cannot convert error result when the original result represents a success.");

    /// <summary>
    ///     Wraps the existing error if it is a failed result and the error is of the specified type otherwise returns the
    ///     existing result object
    /// </summary>
    /// <param name="result"></param>
    /// <param name="errorWrapper">function to wrap the error</param>
    /// <typeparam name="TError">expected error type to wrap</typeparam>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Result<T> WrapError<T, TError>(this Result<T> result, Func<TError, IError> errorWrapper)
        where T : notnull where TError : IError => result is { Failed: true, Error: TError error }
        ? Result<T>.Fail(errorWrapper(error))
        : result;


    /// <summary>
    ///     Converts a nullable reference to a result, representing a success or failure state.
    /// </summary>
    /// <typeparam name="TIn">The type of the nullable input reference.</typeparam>
    /// <param name="i">The nullable input reference.</param>
    /// <param name="error">The error to use if the input reference is null.</param>
    /// <returns>A result representing the input reference if it is not null, or a failed result with the specified error.</returns>
    public static Result<TIn> ToResult<TIn>(this TIn? i, IError error) where TIn : notnull =>
        Result<TIn>.Unknown(i, error);

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