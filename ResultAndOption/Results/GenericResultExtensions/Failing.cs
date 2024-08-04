using ResultAndOption.Errors;
using ResultAndOption.Results.Commands;

namespace ResultAndOption.Results.GenericResultExtensions;

public static class Failing
{
    public static IEnumerable<IError> AggregateErrors(params IResult[] results) => results
        .Where(r => r.Failed)
        .Select(failed => failed.Error);
    
    /// <summary>
    /// Executes the action in case the result is in a failed state.
    /// </summary>
    /// <param name="command">The action to execute</param>
    /// <returns>The same result</returns>
    public static Result<T> OnError<T>(this in Result<T> result, ICommand command) where T : notnull
    {
        if (result.Failed)
        {
            command.Do();
        }

        return result;
    }

    /// <summary>
    /// Executes an action asynchronously in case the result is in a failed state.
    /// </summary>
    /// <param name="command"></param>
    /// <returns>A task of the same result.</returns>
    public static async Task<Result<T>> OnErrorAsync<T>(this Result<T> result, IAsyncCommand command, CancellationToken? token = null) where T : notnull
    {
        if (result.Failed)
        {
            await command.Do(token);
        }

        return result;
    }

    /// <summary>
    /// Executes an action in case the result is in a failed state by taking the error as the only parameter.
    /// </summary>
    /// <param name="command"></param>
    /// <returns>The same result.</returns>
    public static Result<T> OnError<T>(this in Result<T> result, ICommand<IError> command) where T : notnull
    {
        if (result.Failed)
        {
            command.Do(result.Error);
        }

        return result;
    }

    /// <summary>
    /// Executes an action asynchronously in case the result is in a failed state by taking the error as the only parameter.
    /// </summary>
    /// <param name="command"></param>
    /// <returns>A Task of the same result.</returns>
    public static async Task<Result<T>> OnErrorAsync<T>(this Result<T> result, IAsyncCommand<IError> command, CancellationToken? token = null) where T : notnull
    {
        if (result.Failed)
        {
            await command.Do(result.Error, token);
        }

        return result;
    }

    /// <summary>
    ///     Executes the specified action if the result represents a failure.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    /// <returns>The same result after executing the action.</returns>
    public static Result<T> OnError<T>(this in Result<T> result, Action<IError> action) where T : notnull
    {
        if (result.Failed) action(result.Error);
        return result;
    }

    /// <summary>
    ///     Executes the specified action if the result represents a failure
    /// </summary>
    /// <param name="action">Action to execute</param>
    /// <typeparam name="T"></typeparam>
    /// <returns>The same result</returns>
    public static Result<T> OnError<T>(this in Result<T> result, Action action) where T : notnull
    {
        if (result.Failed) action();
        return result;
    } 
}