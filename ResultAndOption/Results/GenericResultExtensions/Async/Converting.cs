using ResultAndOption.Errors;
using ResultAndOption.Options;

namespace ResultAndOption.Results.GenericResultExtensions.Async;

public static class Converting {
    public static async Task<Result<T>> ToResultAsync<T>(this Task<Option<T>> option, IError error) where T : notnull {
        Option<T> data = await option;
        return data.ToResult(error);
    }

    public static async Task<Result<T>> ToResultAsync<T>(this Task<T?> nullable, IError error) where T : notnull {
        var data = await nullable;
        return data.ToResult(error);
    }
}