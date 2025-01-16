using ResultAndOption.Results.Commands;
using ResultAndOption.Results.Getters;
using ResultAndOption.Results.Mappers;

namespace ResultAndOption.Results.SimpleResultExtensions;

/// <summary>
/// Extensions for running actions on simple results
/// </summary>
public static class Doing
{
    /// <summary>
    /// Runs a command if the result is in a success state
    /// </summary>
    /// <param name="result">The result to check</param>
    /// <param name="resultCommand">The result command to run</param>
    /// <returns>The command result</returns>
    public static Result Do(this in Result result, IResultCommand resultCommand) => result.Failed
        ? result
        : resultCommand.Do();

    /// <summary>
    /// Runs an asynchronous command if the result is in a success state
    /// </summary>
    /// <param name="result">The result to check</param>
    /// <param name="resultCommand">The command to run</param>
    /// <param name="token">The cancellation token</param>
    /// <returns>A task of the command result</returns>
    public static Task<Result> DoAsync(
        this in Result result,
        IAsyncResultCommand resultCommand,
        CancellationToken? token = null) => result.Failed
        ? Task.FromResult(result)
        : resultCommand.Do(token);

    /// <summary>
    /// Runs a command if the result is in a success state
    /// </summary>
    /// <param name="result">The result to check</param>
    /// <param name="command">The command to run</param>
    /// <returns>The same result</returns>
    public static Result Do(this in Result result, ICommand command)
    {
        if (result.Succeeded)
        {
            command.Do();
        }

        return result;
    }

    /// <summary>
    /// Runs an asynchronous command if the result is in a success state
    /// </summary>
    /// <param name="result">The result to check</param>
    /// <param name="command">The command to run</param>
    /// <returns>The same result</returns>
    public static async Task<Result> DoAsync(this Result result, IAsyncCommand command)
    {
        if (result.Succeeded)
        {
            await command.Do();
        }

        return result;
    }

    /// <summary>
    ///     Maps the result using the specified function.
    /// </summary>
    /// <param name="result"></param>
    /// <param name="action">The function to map the result.</param>
    /// <returns>
    ///     A new result produced by the function if the original result represents a success. Otherwise, the original
    ///     result is returned.
    /// </returns>
    public static Result Do(this in Result result, Func<Result> action) => result.Failed
        ? result
        : action();

    /// <summary>
    /// Invokes selected action if the result is a success and returns same result
    /// </summary>
    /// <param name="result"></param>
    /// <param name="action">Action to invoke</param>
    /// <returns></returns>
    public static Result Do(this in Result result, Action action)
    {
        if (result.Succeeded)
        {
            action();
        }

        return Result.Ok();
    }
}