using ResultAndOption.Results.Getters;

namespace ResultAndOption.Results.SimpleResultExtensions;

public static class Getting
{
    /// <summary>
    ///     Maps the result using the specified function.
    /// </summary>
    /// <typeparam name="T">The type of data carried by the new result.</typeparam>
    /// <param name="result"></param>
    /// <param name="function">The function to map the result.</param>
    /// <returns>
    ///     A new result of type <typeparamref name="T" /> produced by the function if the original result represents a
    ///     success. Otherwise, a failed result with the same error as the original result is returned.
    /// </returns>
    public static Result<T> Get<T>(this in Result result, Func<Result<T>> function) where T : notnull => result.Failed
        ? Result<T>.Fail(result.Error)
        : function();

    /// <summary>
    ///     Maps the result using the specified function.
    /// </summary>
    /// <typeparam name="T">The type of data carried by the new result.</typeparam>
    /// <param name="result"></param>
    /// <param name="function">The function to map the result.</param>
    /// <returns>
    ///     A new result of type <typeparamref name="T" /> produced by the function if the original result represents a
    ///     success. Otherwise, a failed result with the same error as the original result is returned.
    /// </returns>
    public static Result<T> Get<T>(this in Result result, Func<T> function) where T : notnull => result.Failed
        ? Result<T>.Fail(result.Error)
        : Result<T>.Ok(function()); 
    
    public static Result<TOut> Get<TOut>(this in Result result, IResultGetter<TOut> getter) where TOut : notnull => result.Failed
        ? Result<TOut>.Fail(result.Error)
        : getter.Get();
    
    public static Task<Result<TOut>> GetAsync<TOut>(this in Result result, IAsyncResultGetter<TOut> getter, CancellationToken? token = null) where TOut : notnull => result.Failed
        ? Task.FromResult(Result<TOut>.Fail(result.Error))
        : getter.Get(token);
}