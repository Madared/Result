using ResultAndOption.Errors;
using ResultAndOption.Results.GenericResultExtensions;
using ResultAndOption.Results;

namespace ResultAndOption.Results;
/// <summary>
/// A structured list of results
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class ResultList<T> where T : notnull
{
    /// <summary>
    /// public constructor
    /// </summary>
    public ResultList()
    {
        Successes = new List<T>();
        Errors = new List<CustomError>();
    }

    private ResultList(List<CustomError> errors)
    {
        Successes = new List<T>();
        Errors = errors;
    }

    /// <summary>
    /// All the successful results
    /// </summary>
    public List<T> Successes { get; }
    /// <summary>
    /// All the failed results
    /// </summary>
    public List<CustomError> Errors { get; }

    /// <summary>
    /// Adds a result.
    /// </summary>
    /// <param name="result"></param>
    public void AddResult(Result<T> result)
    {
        switch (result.Failed)
        {
            case true:
                Errors.Add(result.CustomError);
                break;
            default:
                Successes.Add(result.Data);
                break;
        }
    }

    /// <summary>
    /// Adds multiple results
    /// </summary>
    /// <param name="results"></param>
    public void AddResults(IEnumerable<Result<T>> results)
    {
        foreach (Result<T> result in results) AddResult(result);
    }

    /// <summary>
    /// Checks if any of the results are in a failed state
    /// </summary>
    /// <returns></returns>
    public bool HasErrors() => Errors.Count > 0;

    /// <summary>
    /// Maps the results in the list if no errors are found
    /// </summary>
    /// <param name="function">The mapping function</param>
    /// <typeparam name="TOut">The type of the output result list</typeparam>
    /// <returns></returns>
    public ResultList<TOut> Map<TOut>(Func<T, Result<TOut>> function) where TOut : notnull => HasErrors()
        ? new ResultList<TOut>(Errors)
        : Successes
            .Select(function)
            .ToResultList();

    /// <summary>
    /// Maps the results if no errors are found, and maps the nulls into a failed result with the specified error
    /// </summary>
    /// <param name="function">The mapping function</param>
    /// <param name="nullabilityCustomError">The error for nulls</param>
    /// <typeparam name="TOut">the output type of the result list</typeparam>
    /// <returns></returns>
    public ResultList<TOut> Map<TOut>(Func<T, TOut?> function, CustomError nullabilityCustomError) where TOut : notnull =>
        HasErrors()
            ? new ResultList<TOut>(Errors)
            : Successes
                .Select(data => function(data).ToResult(nullabilityCustomError))
                .ToResultList();
    /// <summary>
    /// Maps the results if no errors are found, and maps the nulls into a failed result with the specified error
    /// </summary>
    /// <param name="function">The mapping function</param>
    /// <param name="nullabilityCustomError">The error for nulls</param>
    /// <typeparam name="TOut">the output type of the result list</typeparam>
    /// <returns></returns>
    public Result<TOut> MapList<TOut>(Func<IEnumerable<T>, TOut?> function, CustomError nullabilityCustomError)
        where TOut : notnull => HasErrors()
        ? Result<TOut>.Fail(new MultipleCustomErrors(Errors))
        : function(Successes).ToResult(nullabilityCustomError);
    
    /// <summary>
    /// Maps the results if no errors are found, and maps the nulls into a failed result with the specified error
    /// </summary>
    /// <param name="function">The mapping function</param>
    /// <typeparam name="TOut">the output type of the result list</typeparam>
    /// <returns></returns>
    public ResultList<TOut> MapList<TOut>(Func<IEnumerable<T>, IEnumerable<Result<TOut>>> function)
        where TOut : notnull => HasErrors()
        ? new ResultList<TOut>(Errors)
        : function(Successes).ToResultList();
    
    /// <summary>
    /// Maps the results if no errors are found, and maps the nulls into a failed result with the specified error
    /// </summary>
    /// <param name="function">The mapping function</param>
    /// <typeparam name="TOut">the output type of the result list</typeparam>
    /// <returns></returns>
    public Result<TOut> MapList<TOut>(Func<IEnumerable<T>, TOut> function) where TOut : notnull => HasErrors()
        ? Result<TOut>.Fail(new MultipleCustomErrors(Errors))
        : Result<TOut>.Ok(function(Successes));

     /// <summary>
        /// Maps the results if no errors are found, and maps the nulls into a failed result with the specified error
        /// </summary>
        /// <param name="function">The mapping function</param>
        /// <typeparam name="TOut">the output type of the result list</typeparam>
        /// <returns></returns>
    public Result<TOut> MapList<TOut>(Func<IEnumerable<T>, Result<TOut>> function) where TOut : notnull => HasErrors()
        ? Result<TOut>.Fail(new MultipleCustomErrors(Errors))
        : function(Successes);
}