namespace ResultAndOption.Options.Extensions.Async;

/// <summary>
/// Contains all methods for converting Async Options
/// </summary>
public static class Converting {
    /// <summary>
    /// Converts any Task of a nullable reference to a Task of Option
    /// </summary>
    /// <param name="nullable"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<Option<T>> ToOptionAsync<T>(this Task<T?> nullable) where T : notnull {
        T? data = await nullable;
        return data.ToOption();
    } 
}