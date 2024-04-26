using ResultAndOption.Errors;

namespace ResultAndOption.Results.GenericResultExtensions;

/// <summary>
/// Contains methods to call actions on results
/// </summary>
public static class Doing
{
    /// <summary>
    ///     Maps the data of the result using the specified function that returns a simple result.
    /// </summary>
    /// <param name="result"></param>
    /// <param name="action">The function.</param>
    /// <returns>A new result.</returns>
    public static Result<T> Do<T>(this Result<T> result, Func<T, Result> action) where T : notnull
    {
        if (result.Failed) return result;
        Result actionResult = action(result.Data);
        return actionResult.Failed ? Result<T>.Fail(actionResult.Error) : result;
    }

    /// <summary>
    ///     if the result is successful calls the specified function otherwise returns a simple failed result wrapping the
    ///     current error
    /// </summary>
    /// <param name="result"></param>
    /// <param name="function">The function</param>
    /// <returns>A new simple result</returns>
    public static Result<T> Do<T>(this Result<T> result, Func<Result> function) where T : notnull
    {
        if (result.Failed) return result;
        Result actionResult = function();
        return actionResult.Failed ? Result<T>.Fail(actionResult.Error) : result;
    }


    /// <summary>
    ///     Applies the specified action to the data of the result if it represents a success.
    /// </summary>
    /// <param name="result"></param>
    /// <param name="function">The action to apply.</param>
    /// <returns>The same result after applying the action.</returns>
    public static Result<T> Do<T>(this Result<T> result, Action<T> function) where T : notnull
    {
        if (result.Succeeded) function(result.Data);
        return result;
    }


}