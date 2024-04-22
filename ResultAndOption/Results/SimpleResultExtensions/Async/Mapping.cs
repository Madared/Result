namespace ResultAndOption.Results.SimpleResultExtensions.Async;

public static class Mapping {
    public static async Task<Result> MapAsync(this Task<Result> result, Func<Task<Result>> asyncMapper) {
        var originalResult = await result;
        return originalResult.Failed ? originalResult : await asyncMapper();
    }

    public static async Task<Result> MapAsync(this Task<Result> result, Func<Result> mapper) {
        var originalResult = await result;
        return originalResult.Failed ? originalResult : mapper();
    }
}