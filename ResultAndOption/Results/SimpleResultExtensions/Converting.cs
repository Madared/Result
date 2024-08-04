using ResultAndOption.Errors;

namespace ResultAndOption.Results.SimpleResultExtensions;

/// <summary>
/// Contains methods to Convert Simple Results
/// </summary>
public static class Converting
{
    /// <summary>
    /// Converts a boolean condition into a result, a false condition will become a failed result with the specified error
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="error"></param>
    /// <returns></returns>
    public static Result ConditionResult(this bool condition, IError error) => condition
        ? Result.Ok()
        : Result.Fail(error);
    
        
    /// <summary>
    /// Wraps the results error if its a failure and the type is the same as the specified type, otherwise returns the same result
    /// </summary>
    /// <param name="result"></param>
    /// <param name="errorWrapper"></param>
    /// <typeparam name="TError"></typeparam>
    /// <returns></returns>
    public static Result WrapError<TError>(this in Result result, Func<TError, IError> errorWrapper)
        where TError : IError => result is { Failed: true, Error: TError error }
        ? Result.Fail(errorWrapper(error))
        : result;
}