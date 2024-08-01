namespace ResultAndOption.Transformers;

/// <summary>
/// Represents an object which can transform a value into a new value.
/// </summary>
/// <typeparam name="TIn">The value to transform.</typeparam>
/// <typeparam name="TOut">The transformed value.</typeparam>
public interface ITransformer<in TIn, out TOut>
{
    /// <summary>
    /// Transforms a value into another.
    /// </summary>
    /// <param name="value">The value to transform.</param>
    /// <returns>The transformed value.</returns>
    TOut Transform(TIn value);
}