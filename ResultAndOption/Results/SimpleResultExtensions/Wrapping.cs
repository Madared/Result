namespace ResultAndOption.Results.SimpleResultExtensions;

public static class Wrapping
{
    public static Result<T> Wrap<T>(this Result result, Func<T> mapper) where T : notnull => result.Failed
        ? Result<T>.Fail(result.Error)
        : Result<T>.Ok(mapper());
}