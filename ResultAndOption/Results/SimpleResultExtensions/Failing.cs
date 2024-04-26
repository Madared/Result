using ResultAndOption.Errors;

namespace ResultAndOption.Results.SimpleResultExtensions;

public static class Failing
{
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