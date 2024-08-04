using ResultAndOption.Errors;

namespace ResultAndOption.Results.GenericResultExtensions;

public static class Wrapping
{
    /// <summary>
    ///     Wraps the existing error if it is a failed result and the error is of the specified type otherwise returns the
    ///     existing result object
    /// </summary>
    /// <param name="errorWrapper">function to wrap the error</param>
    /// <typeparam name="TError">expected error type to wrap</typeparam>
    /// <returns></returns>
    public static Result<T> WrapError<T, TError>(this in Result<T> result, Func<TError, IError> errorWrapper) where T : notnull where TError : IError =>
        result is { Failed: true, Error: TError error }
            ? Result<T>.Fail(errorWrapper(error))
            : result;

    public static Result<TOut> Wrap<TIn, TOut>(this in Result<TIn> result, Func<TIn, TOut> mapper) where TIn : notnull where TOut : notnull => result.Failed
        ? Result<TOut>.Fail(result.Error)
        : Result<TOut>.Ok(mapper(result.Data));

    public static Result<TOut> Wrap<TIn, TOut>(this in Result<TIn> result, Func<TOut> mapper) where TIn : notnull where TOut : notnull => result.Failed
        ? Result<TOut>.Fail(result.Error)
        : Result<TOut>.Ok(mapper());
}