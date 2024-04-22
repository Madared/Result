namespace ResultAndOption;

/// <summary>
/// Contains Pipe method
/// </summary>
public static class Piping {
    /// <summary>
    ///     Invokes a function on a non-null reference and returns the result.
    /// </summary>
    /// <typeparam name="TIn">The type of the non-null input reference.</typeparam>
    /// <typeparam name="TOut">The type of the output result.</typeparam>
    /// <param name="i">The non-null input reference.</param>
    /// <param name="function">The function to invoke on the input reference.</param>
    /// <returns>The result of type <typeparamref name="TOut" /> produced by the function.</returns>
    public static TOut Pipe<TIn, TOut>(this TIn i, Func<TIn, TOut> function) where TIn : notnull => function(i);
}