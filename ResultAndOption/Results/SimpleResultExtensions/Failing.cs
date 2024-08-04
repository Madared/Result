using ResultAndOption.Errors;
using ResultAndOption.Results.Commands;

namespace ResultAndOption.Results.SimpleResultExtensions;

public static class Failing
{
    /// <summary>
    ///     Runs an action if the result represents a failure state and returns the same result.
    /// </summary>
    /// <param name="result"></param>
    /// <param name="action">The action to run, accepting the current result as a parameter.</param>
    /// <returns>The same result after running the action.</returns>
    public static Result OnError(this in Result result, Action<IError> action)
    {
        if (result.Failed)
        {
            action(result.Error);
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

    public static Result OnError(this in Result result, ICommand command)
    {
        if (result.Failed)
        {
            command.Do();
        }

        return result;
    }

    public static Result OnError(this in Result result, ICommand<IError> command)
    {
        if (result.Failed)
        {
            command.Do(result.Error);
        }

        return result;
    }

    public static Task<Result> OnErrorAsync(this in Result result, IAsyncCommand command)
    {
        if (result.Failed)
        {
            command.Do();
        }

        return Task.FromResult(result);
    }

    public static Task<Result> OnErrorAsync(this in Result result, IAsyncCommand<IError> command)
    {
        if (result.Failed)
        {
            command.Do(result.Error);
        }

        return Task.FromResult(result);
    }

}