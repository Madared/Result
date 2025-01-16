using ResultAndOption.Results.Commands;

namespace ResultAndOption.Results.GenericResultExtensions;

/// <summary>
/// Extension methods for executing commands on generic results
/// </summary>
public static class Doing
{
    /// <summary>
    /// Executes an action if the result is successful otherwise does nothing. Returns the same result.
    /// </summary>
    /// <param name="result">The result to check</param>
    /// <param name="command">The command to run</param>
    /// <returns>The same result.</returns>
    public static Result Do<T>(this in Result<T> result, ICommand<T> command) where T : notnull
    {
        if (result.Succeeded)
        {
            command.Do(result.Data);
        }

        return result.ToSimpleResult();
    }

    /// <summary>
    /// Executes an action asynchronously if the result is successful otherwise does nothing. Returns the same result.
    /// </summary>
    /// <param name="result">The result to check</param>
    /// <param name="command">The command to run</param>
    /// <param name="token">The cancellation token</param>
    /// <returns>A Task of the same result.</returns>
    public static async Task<Result> DoAsync<T>(
        this Result<T> result,
        IAsyncCommand<T> command,
        CancellationToken? token = null) where T : notnull
    {
        if (result.Succeeded)
        {
            await command.Do(result.Data, token);
        }

        return result.ToSimpleResult();
    }

    /// <summary>
    /// Performs a command if the current result is a success and returns its result otherwise passes the Error into a new SimpleResult
    /// </summary>
    /// <param name="result">The result to check</param>
    /// <param name="command">The command to execute</param>
    /// <returns>The result of running the command</returns>
    public static Result Do<T>(this in Result<T> result, IResultCommand<T> command) where T : notnull =>
        result.Succeeded
            ? command.Do(result.Data)
            : Result.Fail(result.CustomError);

    /// <summary>
    /// Performs an async command if the current result is a success and returns its result otherwise passes the Error into a new SimpleResult
    /// </summary>
    /// <param name="result">The result to check</param>
    /// <param name="command">the async command to execute</param>
    /// <param name="token">The cancellation token</param>
    /// <returns>The result of running the command</returns>
    public static async Task<Result> DoAsync<T>(
        this Result<T> result,
        IAsyncResultCommand<T> command,
        CancellationToken? token = null) where T : notnull => result.Succeeded
        ? await command.Do(result.Data, token)
        : Result.Fail(result.CustomError);

    /// <summary>
    /// Maps the data of the result using the specified function that returns a simple result.
    /// </summary>
    /// <param name="result">The result to check</param>
    /// <param name="action">The function.</param>
    /// <returns>The result of running the action</returns>
    public static Result Do<T>(this in Result<T> result, Func<T, Result> action) where T : notnull
    {
        if (result.Failed)
        {
            return Result.Fail(result.CustomError);
        }

        Result actionResult = action(result.Data);
        return result.ToSimpleResult();
    }

    /// <summary>
    ///     if the result is successful calls the specified function otherwise returns a simple failed result wrapping the
    ///     current error
    /// </summary>
    /// <param name="result">The result to check</param>
    /// <param name="function">The function</param>
    /// <returns>A new simple result</returns>
    public static Result Do<T>(this in Result<T> result, Func<Result> function) where T : notnull => result.Succeeded
        ? function()
        : Result.Fail(result.CustomError);

    /// <summary>
    ///     Applies the specified action to the data of the result if it represents a success.
    /// </summary>
    /// <param name="result">The result to check</param>
    /// <param name="function">The action to apply.</param>
    /// <returns>The same result after applying the action.</returns>
    public static Result Do<T>(this in Result<T> result, Action<T> function) where T : notnull

    {
        if (result.Succeeded) function(result.Data);
        return Result.Fail(result.CustomError);
    }
}