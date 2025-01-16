namespace ResultAndOption.Results.Getters;

/// <summary>
/// A result getter
/// </summary>
/// <typeparam name="T">The result type</typeparam>
public interface IResultGetter<T> where T : notnull
{
    /// <summary>
    /// Returns a generic result
    /// </summary>
    /// <returns></returns>
    Result<T> Get();
}