using ResultAndOption.Errors;
using ResultAndOption.Results.Commands;

namespace ResultAndOption.Results.SimpleResultExtensions;

/// <summary>
/// Extension methods for handling failure and errors on simple results
/// </summary>
public static class Failing
{
    /// <summary>
    ///     Runs an action if the result represents a failure state and returns the same result.
    /// </summary>
    /// <param name="result"></param>
    /// <param name="action">The action to run, accepting the current result as a parameter.</param>
    /// <returns>The same result after running the action.</returns>
    public static Result OnError(this in Result result, Action<CustomError> action)
    {
        if (result.Failed)
        {
            action(result.CustomError);
        }
        return result;
    }

    /// <summary>
    /// If the result is a failure calls the specified action
    /// </summary>
    /// <param name="result"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static Result OnError(this in Result result,Action action)
    {
        if (result.Failed)
        {
            action();
        }
        return result;
    } 

    /// <summary>
    /// Performs a command if the result is in a failed state
    /// </summary>
    /// <param name="result">The result to check</param>
    /// <param name="command">The command to run</param>
    /// <returns>The same result</returns>
    public static Result OnError(this in Result result, ICommand command)
    {
        if (result.Failed)
        {
            command.Do();
        }

        return result;
    }

    /// <summary>
    /// Performs a command if the result is in a failed state
    /// </summary>
    /// <param name="result">The result to check</param>
    /// <param name="command">The command to run</param>
    /// <returns>The same result</returns>
    public static Result OnError(this in Result result, ICommand<CustomError> command)
    {
        if (result.Failed)
        {
            command.Do(result.CustomError);
        }

        return result;
    }

    /// <summary>
    /// Performs an asynchronous command if the result is in a failed state
    /// </summary>
    /// <param name="result">The result to check</param>
    /// <param name="command">The asynchronous command to run</param>
    /// <returns>A task of the same result</returns>
    public static async Task<Result> OnErrorAsync(this Result result, IAsyncCommand command)
    {
        if (result.Failed)
        {
            await command.Do();
        }

        return result;
    }

    /// <summary>
    /// Performs an asynchronous command if the result is in a failed state
    /// </summary>
    /// <param name="result">The result to check</param>
    /// <param name="command">The asynchronous command to run</param>
    /// <returns>A task of the same result</returns>
    public static async Task<Result> OnErrorAsync(this Result result, IAsyncCommand<CustomError> command)
    {
        if (result.Failed)
        {
            await command.Do(result.CustomError);
        }

        return result;
    }

}