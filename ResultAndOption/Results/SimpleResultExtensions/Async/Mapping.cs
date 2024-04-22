namespace ResultAndOption.Results.SimpleResultExtensions.Async;

public static class Mapping {
    public static async Task<Result> MapAsync(this Task<Result> result, Func<Task<Result>> asyncMapper) {
        Result originalResult = await result;
        return originalResult.Failed ? originalResult : await asyncMapper();
    }

    public static async Task<Result> MapAsync(this Task<Result> result, Func<Result> mapper) {
        Result originalResult = await result;
        return originalResult.Failed ? originalResult : mapper();
    }
    
    public static async Task<Result> MapAsync<T>(this Result result, Func<Task<Result>> mapper) where T : notnull => result.Failed 
        ? Result.Fail(result.Error) 
        : await mapper();
}