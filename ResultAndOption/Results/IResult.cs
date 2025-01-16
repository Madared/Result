using ResultAndOption.Errors;

namespace ResultAndOption.Results;

/// <summary>
/// Represents the most basic structure of a result
/// </summary>
public interface IResult
{
    /// <summary>
    /// Tells you if the results was successful
    /// </summary>
    bool Succeeded { get; }
    /// <summary>
    /// Tells you if the result failed
    /// </summary>
    bool Failed { get; }
    /// <summary>
    /// If the result failed will contain the custom error/ reason for the failed result
    /// </summary>
    CustomError CustomError { get; }
}

/// <summary>
/// Represents a result which contains some data if it is successful
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IResult<out T> : IResult
{
    /// <summary>
    /// Will contain the data if the result is successful
    /// </summary>
    T Data { get; }
}