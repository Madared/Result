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
}