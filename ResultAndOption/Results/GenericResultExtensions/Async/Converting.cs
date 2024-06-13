using ResultAndOption.Errors;
using ResultAndOption.Options;

namespace ResultAndOption.Results.GenericResultExtensions.Async;

/// <summary>
/// Contains all methods to Convert to a result Async
/// </summary>
public static class Converting
{
    /// <summary>
    /// Awaits for the option and calls the ToResult method
    /// </summary>
    /// <param name="option"></param>
    /// <param name="error"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<Result<T>> ToResultAsync<T>(this Task<Option<T>> option, IError error) where T : notnull
    {
        Option<T> data = await option;
        return data.ToResult(error);
    }

    /// <summary>
    /// Awaits for the nullable reference and calls the ToResult method
    /// </summary>
    /// <param name="nullable"></param>
    /// <param name="error"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<Result<T>> ToResultAsync<T>(this Task<T?> nullable, IError error) where T : notnull
    {
        T? data = await nullable;
        return data.ToResult(error);
    }

    /// <summary>
    /// Turns a task of a generic result into a task of a simple result
    /// </summary>
    /// <param name="result"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<Result> ToSimpleResultAsync<T>(this Task<Result<T>> result) where T : notnull
    {
        Result<T> originalResult = await result;
        return originalResult.Failed ? Result.Fail(originalResult.Error) : Result.Ok();
    }
}