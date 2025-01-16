using ResultAndOption.Errors;
using ResultAndOption.Results.Commands;

namespace ResultAndOption.Results.GenericResultExtensions;

/// <summary>
/// Extension methods for handling failures and errors on generic results
/// </summary>
public static class Failing
{
    /// <summary>
    /// Aggregates multiple results(will box every single result USE SPARINGLY)
    /// </summary>
    /// <param name="results">The results to aggregate</param>
    /// <returns></returns>
    public static IEnumerable<CustomError> AggregateErrors(params IResult[] results) => results
        .Where(r => r.Failed)
        .Select(failed => failed.CustomError);
    
    /// <summary>
    /// Checks if any of the results in the enumerable are in a failing state, and if not returns an enumerable
    /// of the internal data of each result wrapped in a result, otherwise will return a failed result
    /// </summary>
    /// <param name="results"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Result<IEnumerable<T>> MapResultListToSingle<T>(this IEnumerable<Result<T>> results) where T : notnull
    {
        List<Result<T>> enumeratedResults = results.ToList();
        IEnumerable<CustomError> errors = enumeratedResults.Where(r => r.Failed).Select(r => r.CustomError).ToList();
        if (errors.Any())
        {
            return Result<IEnumerable<T>>.Fail(new MultipleCustomErrors(errors));
        }

        IEnumerable<T> data = enumeratedResults.Select(r => r.Data);
        return Result<IEnumerable<T>>.Ok(data);
    }

    /// <summary>
    /// Executes the action in case the result is in a failed state.
    /// </summary>
    /// <param name="result">The result to check</param>
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
    /// <param name="result">The result to check</param>
    /// <param name="command">The command to run</param>
    /// <param name="token">The cancellation token</param>
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
    /// <param name="result">The result to check</param>
    /// <param name="command">The command to run</param>
    /// <returns>The same result.</returns>
    public static Result<T> OnError<T>(this in Result<T> result, ICommand<CustomError> command) where T : notnull
    {
        if (result.Failed)
        {
            command.Do(result.CustomError);
        }

        return result;
    }

    /// <summary>
    /// Executes an action asynchronously in case the result is in a failed state by taking the error as the only parameter.
    /// </summary>
    /// <param name="result">The reuslt to check</param>
    /// <param name="command">The command to run</param>
    /// <param name="token">The cancellation token</param>
    /// <returns>A Task of the same result.</returns>
    public static async Task<Result<T>> OnErrorAsync<T>(this Result<T> result, IAsyncCommand<CustomError> command, CancellationToken? token = null) where T : notnull
    {
        if (result.Failed)
        {
            await command.Do(result.CustomError, token);
        }

        return result;
    }

    /// <summary>
    ///     Executes the specified action if the result represents a failure.
    /// </summary>
    /// <param name="result">The result to check</param>
    /// <param name="action">The action to execute.</param>
    /// <returns>The same result after executing the action.</returns>
    public static Result<T> OnError<T>(this in Result<T> result, Action<CustomError> action) where T : notnull
    {
        if (result.Failed) action(result.CustomError);
        return result;
    }

    /// <summary>
    ///     Executes the specified action if the result represents a failure
    /// </summary>
    /// <param name="result">The result to check</param>
    /// <param name="action">Action to execute</param>
    /// <typeparam name="T"></typeparam>
    /// <returns>The same result</returns>
    public static Result<T> OnError<T>(this in Result<T> result, Action action) where T : notnull
    {
        if (result.Failed) action();
        return result;
    } 
}