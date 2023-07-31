namespace Results;

/// <summary>
/// Provides extension methods for working with reference types and results.
/// </summary>
public static class ReferenceExtensions
{
    /// <summary>
    /// Invokes a function on a nullable reference and returns a result, handling the case when the reference is null.
    /// </summary>
    /// <typeparam name="TIn">The type of the nullable input reference.</typeparam>
    /// <typeparam name="TOut">The type of the output result.</typeparam>
    /// <param name="i">The nullable input reference.</param>
    /// <param name="function">The function to invoke on the input reference.</param>
    /// <returns>A result of type <typeparamref name="TOut"/> produced by the function if the input reference is not null. Otherwise, a failed result with an unknown error is returned.</returns>
    public static Result<TOut> Pipe<TIn, TOut>(this TIn? i, Func<TIn, Result<TOut>> function)
        where TIn : class
        where TOut : class
    {
        return i is null
            ? Result<TOut>.Fail(new UnknownError())
            : function(i);
    }

    /// <summary>
    /// Invokes a function on a value and returns the result, handling the case when the reference is null.
    /// </summary>
    /// <typeparam name="TIn">The type of the nullable input reference.</typeparam>
    /// <typeparam name="TOut">The type of the output result.</typeparam>
    /// <param name="i">The nullable input reference.</param>
    /// <param name="function">The function to invoke on the input reference.</param>
    /// <returns>The result of type <typeparamref name="TOut"/> produced by the function if the input reference is not null. Otherwise, returns null.</returns>
    public static TOut? Pipe<TIn, TOut>(this TIn? i, Func<TIn, TOut?> function)
        where TIn : notnull
        where TOut : notnull
    {
        return i is null
            ? default(TOut?)
            : function(i);
    }

    /// <summary>
    /// Invokes a function on a non-null reference and returns the result.
    /// </summary>
    /// <typeparam name="TIn">The type of the non-null input reference.</typeparam>
    /// <typeparam name="TOut">The type of the output result.</typeparam>
    /// <param name="i">The non-null input reference.</param>
    /// <param name="function">The function to invoke on the input reference.</param>
    /// <returns>The result of type <typeparamref name="TOut"/> produced by the function.</returns>
    public static TOut PipeNonNull<TIn, TOut>(this TIn i, Func<TIn, TOut> function)
    {
        return function(i);
    }

    /// <summary>
    /// Invokes a function on a nullable reference and returns a result, handling the case when the reference is null.
    /// </summary>
    /// <typeparam name="TIn">The type of the nullable input reference.</typeparam>
    /// <param name="i">The nullable input reference.</param>
    /// <param name="function">The function to invoke on the input reference.</param>
    /// <param name="error">The error to use if the input reference is null.</param>
    /// <returns>A result produced by the function if the input reference is not null. Otherwise, a failed result with the specified error is returned.</returns>
    public static Result Pipe<TIn>(this TIn? i, Func<TIn, Result> function, IError error)
        where TIn : notnull
    {
        return i is null
            ? Result.Fail(error)
            : function(i);
    }

    /// <summary>
    /// Converts a nullable reference to a result, representing a success or failure state.
    /// </summary>
    /// <typeparam name="TIn">The type of the nullable input reference.</typeparam>
    /// <param name="i">The nullable input reference.</param>
    /// <param name="error">The error to use if the input reference is null.</param>
    /// <returns>A result representing the input reference if it is not null, or a failed result with the specified error.</returns>
    public static Result<TIn> ToResult<TIn>(this TIn? i, IError error)
        where TIn : notnull
    {
        return Result<TIn>.Unknown(i, error);
    }

    /// <summary>
    /// Converts a simple list of results to the more specific ResultList.
    /// </summary>
    /// <param name="results">The list of results to convert</param>
    /// <typeparam name="TIn">The type of data carried by the result</typeparam>
    /// <returns>A ResultList</returns>
    public static ResultList<TIn> ToResult<TIn>(this List<Result<TIn>> results)
        where TIn : notnull
    {
        ResultList<TIn> resultList = new();
        resultList.AddResults(results);
        return resultList;
    }

    /// <summary>
    /// Applies a mapping function to each element in a list and returns a list of the mapped results.
    /// </summary>
    /// <typeparam name="TIn">The type of the elements in the input list.</typeparam>
    /// <typeparam name="TOut">The type of the elements in the output list.</typeparam>
    /// <param name="list">The input list.</param>
    /// <param name="function">The mapping function to apply to each element in the list.</param>
    /// <returns>A list of the mapped values.</returns>
    public static List<TOut> ListMap<TIn, TOut>(this IEnumerable<TIn> list, Func<TIn, TOut> function) =>
        list
            .Select(function)
            .ToList();
}