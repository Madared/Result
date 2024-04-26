using ResultAndOption.Errors;

namespace ResultAndOption.Results.SimpleResultExtensions;

/// <summary>
/// Contains methods to call actions on simple results
/// </summary>
public static class Doing
{



    /// <summary>
    ///     Maps the result using the specified function.
    /// </summary>
    /// <param name="result"></param>
    /// <param name="action">The function to map the result.</param>
    /// <returns>
    ///     A new result produced by the function if the original result represents a success. Otherwise, the original
    ///     result is returned.
    /// </returns>
    public static Result Do(this Result result, Func<Result> action) => result.Failed
        ? result
        : action();

    /// <summary>
    /// Invokes selected action if the result is a success and returns same result
    /// </summary>
    /// <param name="result"></param>
    /// <param name="action">Action to invoke</param>
    /// <returns></returns>
    public static Result Do(this Result result, Action action)
    {
        if (result.Failed) return result;
        action();
        return Result.Ok();
    }

    /// <summary>
    ///     Runs an action if the result represents a failure state and returns the same result.
    /// </summary>
    /// <param name="result"></param>
    /// <param name="action">The action to run, accepting the current result as a parameter.</param>
    /// <returns>The same result after running the action.</returns>
    public static Result IfFailed(this Result result, Action<IError> action)
    {
        if (result.Failed) action(result.Error);
        return result;
    }

    /// <summary>
    /// If the result is a failure calls the specified action
    /// </summary>
    /// <param name="result"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static Result IfFailed(this Result result, Action action)
    {
        if (result.Failed) action();
        return result;
    }
}