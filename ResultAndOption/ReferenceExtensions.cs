namespace Results;

/// <summary>
///     Provides extension methods for working with reference types and results.
/// </summary>
public static class ReferenceExtensions {
    /// <summary>
    ///     Invokes a function on a non-null reference and returns the result.
    /// </summary>
    /// <typeparam name="TIn">The type of the non-null input reference.</typeparam>
    /// <typeparam name="TOut">The type of the output result.</typeparam>
    /// <param name="i">The non-null input reference.</param>
    /// <param name="function">The function to invoke on the input reference.</param>
    /// <returns>The result of type <typeparamref name="TOut" /> produced by the function.</returns>
    public static TOut Pipe<TIn, TOut>(this TIn i, Func<TIn, TOut> function) where TIn : notnull {
        return function(i);
    }


    /// <summary>
    ///     Converts a nullable reference to a result, representing a success or failure state.
    /// </summary>
    /// <typeparam name="TIn">The type of the nullable input reference.</typeparam>
    /// <param name="i">The nullable input reference.</param>
    /// <param name="error">The error to use if the input reference is null.</param>
    /// <returns>A result representing the input reference if it is not null, or a failed result with the specified error.</returns>
    public static Result<TIn> ToResult<TIn>(this TIn? i, IError error) where TIn : notnull {
        return Result<TIn>.Unknown(i, error);
    }

    /// <summary>
    ///     Converts a simple list of results to the more specific ResultList.
    /// </summary>
    /// <param name="results">The list of results to convert</param>
    /// <typeparam name="TIn">The type of data carried by the result</typeparam>
    /// <returns>A ResultList</returns>
    public static ResultList<TIn> ToResultList<TIn>(this IEnumerable<Result<TIn>> results)
        where TIn : notnull {
        ResultList<TIn> resultList = new();
        resultList.AddResults(results);
        return resultList;
    }

    /// <summary>
    ///     Generates an option type based on a null reference type
    /// </summary>
    /// <param name="data"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Option<T> ToOption<T>(this T? data) where T : notnull {
        return Option<T>.Maybe(data);
    }

    public static Result<T> ToResult<T>(this Option<T> data, IError error) where T : notnull => data.IsNone()
        ? Result<T>.Fail(error)
        : Result<T>.Ok(data.Data);

    public static Result ConditionResult(this bool condition, IError error) => condition
        ? Result.Ok()
        : Result.Fail(error);
}