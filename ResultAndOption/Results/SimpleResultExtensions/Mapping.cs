namespace ResultAndOption.Results.SimpleResultExtensions;

/// <summary>
/// Contains methods to map simple results
/// </summary>
public static class Mapping {
    /// <summary>
    ///     Maps the result using the specified function.
    /// </summary>
    /// <typeparam name="T">The type of data carried by the new result.</typeparam>
    /// <param name="result"></param>
    /// <param name="function">The function to map the result.</param>
    /// <returns>
    ///     A new result of type <typeparamref name="T" /> produced by the function if the original result represents a
    ///     success. Otherwise, a failed result with the same error as the original result is returned.
    /// </returns>
    public static Result<T> Map<T>(this Result result, Func<Result<T>> function) where T : notnull => result.Failed 
        ? Result<T>.Fail(result.Error) 
        : function();

    /// <summary>
    ///     Maps the result using the specified function.
    /// </summary>
    /// <typeparam name="T">The type of data carried by the new result.</typeparam>
    /// <param name="result"></param>
    /// <param name="function">The function to map the result.</param>
    /// <returns>
    ///     A new result of type <typeparamref name="T" /> produced by the function if the original result represents a
    ///     success. Otherwise, a failed result with the same error as the original result is returned.
    /// </returns>
    public static Result<T> Map<T>(this Result result, Func<T> function) where T : notnull => result.Failed 
        ? Result<T>.Fail(result.Error) 
        : Result<T>.Ok(function());
}