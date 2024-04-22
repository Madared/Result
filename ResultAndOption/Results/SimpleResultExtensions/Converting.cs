using ResultAndOption.Errors;

namespace ResultAndOption.Results.SimpleResultExtensions;

public static class Converting {
    public static Result WrapError<TError>(this Result result, Func<TError, IError> errorWrapper) where TError : IError => result is { Failed: true, Error: TError error }
        ? Result.Fail(errorWrapper(error))
        : result;

    public static Result ConditionResult(this bool condition, IError error) => condition
        ? Result.Ok()
        : Result.Fail(error);
}