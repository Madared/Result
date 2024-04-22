namespace ResultAndOption.Options.Extensions.Async;

public static class Converting {
    public static async Task<Option<T>> ToOptionAsync<T>(this Task<T?> nullable) where T : notnull {
        T? data = await nullable;
        return data.ToOption();
    } 
}