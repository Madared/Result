using ResultAndOption.Errors;

namespace ResultAndOption.Results.GenericResultExtensions;

public static class Failing
{
    /// <summary>
    ///     Executes the specified action if the result represents a failure.
    /// </summary>
    /// <param name="result"></param>
    /// <param name="action">The action to execute.</param>
    /// <returns>The same result after executing the action.</returns>
    public static Result<T> OnError<T>(this Result<T> result, Action<IError> action) where T : notnull
    {
        if (result.Failed) action(result.Error);
        return result;
    }

    /// <summary>
    ///     Executes the specified action if the result represents a failure
    /// </summary>
    /// <param name="result"></param>
    /// <param name="action">Action to execute</param>
    /// <typeparam name="T"></typeparam>
    /// <returns>The same result</returns>
    public static Result<T> OnError<T>(this Result<T> result, Action action) where T : notnull
    {
        if (result.Failed) action();
        return result;
    }
}