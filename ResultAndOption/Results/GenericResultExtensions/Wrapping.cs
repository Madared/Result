using ResultAndOption.Errors;

namespace ResultAndOption.Results.GenericResultExtensions;

public static class Wrapping {
    public static Result<TOut> Wrap<TIn, TOut>(this Result<TIn> result, Func<TIn, TOut> mapper)
        where TIn : notnull where TOut : notnull => result.Failed
        ? Result<TOut>.Fail(result.Error)
        : Result<TOut>.Ok(mapper(result.Data));

    public static Result<TOut> Wrap<TIn, TOut>(this Result<TIn> result, Func<TOut> mapper)
        where TIn : notnull where TOut : notnull => result.Failed
        ? Result<TOut>.Fail(result.Error)
        : Result<TOut>.Ok(mapper());
}