using ResultAndOption.Errors;
using ResultAndOption.Results;

namespace ResultAndOption;

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
    ///     Generates an option type based on a null reference type
    /// </summary>
    /// <param name="data"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static Option<T> ToOption<T>(this T? data) where T : notnull {
        return Option<T>.Maybe(data);
    }
}