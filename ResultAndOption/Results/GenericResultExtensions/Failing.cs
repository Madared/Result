using ResultAndOption.Errors;
using ResultAndOption.Results.Commands;

namespace ResultAndOption.Results.GenericResultExtensions;

public static class Failing
{
    public static IEnumerable<IError> AggregateErrors(params IResult[] results) => results
        .Where(r => r.Failed)
        .Select(failed => failed.Error);
    
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
        IEnumerable<IError> errors = enumeratedResults.Where(r => r.Failed).Select(r => r.Error).ToList();
        if (errors.Any())
        {
            return Result<IEnumerable<T>>.Fail(new MultipleErrors(errors));
        }

        IEnumerable<T> data = enumeratedResults.Select(r => r.Data);
        return Result<IEnumerable<T>>.Ok(data);
    }
    
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