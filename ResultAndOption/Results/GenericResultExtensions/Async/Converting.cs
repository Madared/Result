using ResultAndOption.Errors;
using ResultAndOption.Options;
using ResultAndOption.Results;

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
    /// <param name="customError"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<Result<T>> ToResultAsync<T>(this Task<Option<T>> option, CustomError customError) where T : notnull
    {
        Option<T> data = await option;
        return data.ToResult(customError);
    }

    /// <summary>
    /// Awaits for the nullable reference and calls the ToResult method
    /// </summary>
    /// <param name="nullable"></param>
    /// <param name="customError"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<Result<T>> ToResultAsync<T>(this Task<T?> nullable, CustomError customError) where T : notnull
    {
        T? data = await nullable;
        return data.ToResult(customError);
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
        return originalResult.Failed ? Result.Fail(originalResult.CustomError) : Result.Ok();
    }
}