namespace ResultAndOption.Results.SimpleResultExtensions;

/// <summary>
/// Extensions for handling result wrapping in simple results
/// </summary>
public static class Wrapping
{
    /// <summary>
    /// Returns the output of the mapper wrapped in a result or the current error wrapped in the specified type (allows for result wrapping)
    /// </summary>
    /// <param name="result">The result to check</param>
    /// <param name="mapper">The mapper to run</param>
    /// <typeparam name="T">The result type</typeparam>
    /// <returns>The result obtained from the mapper or the current error wrapped in a result of the specified type</returns>
    public static Result<T> Wrap<T>(this in Result result, Func<T> mapper) where T : notnull => result.Failed
        ? Result<T>.Fail(result.CustomError)
        : Result<T>.Ok(mapper());  
}