namespace ResultAndOption.Transformers;

/// <summary>
/// Represents an object which can transform a value into a new value asynchronously.
/// </summary>
/// <typeparam name="TIn">The value to transform.</typeparam>
/// <typeparam name="TOut">The transformed value.</typeparam>
public interface IAsyncTransformer<in TIn, TOut>
{
    /// <summary>
    /// Transforms a value into another asynchronously.
    /// </summary>
    /// <param name="value">The value to transform.</param>
    /// <returns>A task of the transformed value.</returns>
    Task<TOut> TransformAsync(TIn value);
}