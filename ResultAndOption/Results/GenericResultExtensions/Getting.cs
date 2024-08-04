using ResultAndOption.Results.Getters;
namespace ResultAndOption.Results.GenericResultExtensions;

public static class Getting
{
    /// <summary>
    /// Uses an IMapper to map a successful result data through its transformation without parameters
    /// or into a new failed result with the existing error.
    /// </summary>
    /// <param name="mapper"></param>
    /// <typeparam name="TOut"></typeparam>
    /// <returns>A new result</returns>
    public static Result<TOut> Get<TIn, TOut>(this in Result<TIn> result, IResultGetter<TOut> mapper) where TIn : notnull where TOut : notnull => result.Failed
        ? Result<TOut>.Fail(result.Error)
        : mapper.Get();

    /// <summary>
    /// Uses an IMapper to map a successful result data through its transformation without parameters
    /// /// or into a new failed result with the existing error.
    /// </summary>
    /// <param name="mapper"></param>
    /// <typeparam name="TOut"></typeparam>
    /// <returns>A Task of the new result</returns>
    public static async Task<Result<TOut>> GetAsync<TIn, TOut>(this Result<TIn> result, IAsyncResultGetter<TOut> mapper, CancellationToken? token = null)
        where TIn : notnull where TOut : notnull => result.Failed
        ? Result<TOut>.Fail(result.Error)
        : await mapper.Get(token);
}