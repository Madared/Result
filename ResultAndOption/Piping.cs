using ResultAndOption.Results.Mappers;
using ResultAndOption.Transformers;

namespace ResultAndOption;

/// <summary>
/// Contains Pipe method
/// </summary>
public static class Piping
{
    /// <summary>
    ///     Invokes a function on a non-null reference and returns the result.
    /// </summary>
    /// <typeparam name="TIn">The type of the non-null input reference.</typeparam>
    /// <typeparam name="TOut">The type of the output result.</typeparam>
    /// <param name="i">The non-null input reference.</param>
    /// <param name="function">The function to invoke on the input reference.</param>
    /// <returns>The result of type <typeparamref name="TOut" /> produced by the function.</returns>
    public static TOut Pipe<TIn, TOut>(this TIn i, Func<TIn, TOut> function) where TIn : notnull => function(i);

    /// <summary>
    ///     Awaits the Task and passes it into the mapper function.
    /// </summary>
    /// <param name="i"></param>
    /// <param name="mapper"></param>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    /// <returns></returns>
    public static async Task<TOut> PipeAsync<TIn, TOut>(this Task<TIn> i, Func<TIn, TOut> mapper) where TIn : notnull
    {
        TIn original = await i;
        return mapper(original);
    }

    /// <summary>
    /// Pipes a value into a transformer
    /// </summary>
    /// <param name="value">The value to be transformed</param>
    /// <param name="transformer">The transformer to be used</param>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    /// <returns>The transformed value</returns>
    public static TOut Pipe<TIn, TOut>(this TIn value, ITransformer<TIn, TOut> transformer) =>
        transformer.Transform(value);

    /// <summary>
    /// Pipes a value into an async transformer
    /// </summary>
    /// <param name="value">the value to be transformed</param>
    /// <param name="transformer">the async transformer</param>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    /// <returns>A task of the transformed value</returns>
    public static Task<TOut> PipeAsync<TIn, TOut>(this TIn value, IAsyncTransformer<TIn, TOut> transformer) =>
        transformer.TransformAsync(value);

    /// <summary>
    /// Performs a Transform asynchronously on a task of a value by awaiting it.
    /// </summary>
    /// <param name="value">The task of the value to transform</param>
    /// <param name="transformer">The transformer</param>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    /// <returns>A Task of the transformed value</returns>
    public static async Task<TOut> PipeAsync<TIn, TOut>(this Task<TIn> value, ITransformer<TIn, TOut> transformer)
    {
        TIn original = await value;
        return transformer.Transform(original);
    }

    /// <summary>
    /// Performs an async Transform on a task of a value by awaiting it
    /// </summary>
    /// <param name="value">The task of the value to tranform</param>
    /// <param name="transformer">The transformer</param>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    /// <returns>A task of the transformed value</returns>
    public static async Task<TOut> PipeAsync<TIn, TOut>(this Task<TIn> value, IAsyncTransformer<TIn, TOut> transformer)
    {
        TIn original = await value;
        return await transformer.TransformAsync(original);
    }
}