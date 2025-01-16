using ResultAndOption.Errors;

namespace ResultAndOption.Results.GenericResultExtensions;

/// <summary>
/// Extensions for wrapping generic results
/// </summary>
public static class Wrapping
{
    /// <summary>
    ///     Wraps the existing error if it is a failed result and the error is of the specified type otherwise returns the
    ///     existing result object
    /// </summary>
    /// <param name="result">The result to check</param>
    /// <param name="errorWrapper">function to wrap the error</param>
    /// <typeparam name="TError">expected error type to wrap</typeparam>
    /// <typeparam name="T">The type of the input result</typeparam>
    /// <returns></returns>
    public static Result<T> WrapError<T, TError>(this in Result<T> result, Func<TError, CustomError> errorWrapper) where T : notnull where TError : CustomError =>
        result is { Failed: true, CustomError: TError error }
            ? Result<T>.Fail(errorWrapper(error))
            : result;

    /// <summary>
    /// Returns the result of the mapping function unwrapped regardless of the type, meaning it can lead to wrapped results (ex: result of result of string)
    /// </summary>
    /// <param name="result">The result to check (outer result)</param>
    /// <param name="mapper">The mapper to run</param>
    /// <typeparam name="TIn">The type of the input result</typeparam>
    /// <typeparam name="TOut">The type of the output result</typeparam>
    /// <returns></returns>
    public static Result<TOut> Wrap<TIn, TOut>(this in Result<TIn> result, Func<TIn, TOut> mapper) where TIn : notnull where TOut : notnull => result.Failed
        ? Result<TOut>.Fail(result.CustomError)
        : Result<TOut>.Ok(mapper(result.Data));

    /// <summary>
    /// Returns the result of the mapping function unwrapped regardless of the type, meaning it can lead to wrapped results (ex: result of result of string)
    /// </summary>
    /// <param name="result">The result to check (outer result)</param>
    /// <param name="mapper">The mapper to run</param>
    /// <typeparam name="TIn">The type of the input result</typeparam>
    /// <typeparam name="TOut">The type of the output result</typeparam>
    /// <returns></returns>
    public static Result<TOut> Wrap<TIn, TOut>(this in Result<TIn> result, Func<TOut> mapper) where TIn : notnull where TOut : notnull => result.Failed
        ? Result<TOut>.Fail(result.CustomError)
        : Result<TOut>.Ok(mapper());
}