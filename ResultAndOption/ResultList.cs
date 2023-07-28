namespace Results;

public class ResultList<T> where T : notnull
{
    public List<T> Successes { get; }
    public List<IError> Errors { get; }

    public ResultList()
    {
        Successes = new();
        Errors = new();
    }

    private ResultList(List<IError> errors)
    {
        Successes = new();
        Errors = errors;
    }

    public void AddResult(Result<T> result)
    {
        if (result.Failed)
            Errors.Add(result.Error);
        else
            Successes.Add(result.Data);
    }

    public void AddResults(List<Result<T>> results) =>
        results.ForEach(AddResult);

    public bool HasErrors() => Errors.Count > 0;

    public ResultList<TOut> Map<TOut>(Func<T, Result<TOut>> function) where TOut : notnull
    {
        ResultList<TOut> newList = new(Errors);
        Successes
            .ListMap(function)
            .ForEach(result => newList.AddResult(result));
        return newList;
    }

    public ResultList<TOut> Map<TOut>(Func<T, TOut?> function) where TOut : notnull
    {
        ResultList<TOut> newList = new(Errors);
        Successes
            .ListMap(data => function(data).ToResult(new UnknownError()))
            .ForEach(result => newList.AddResult(result));
        return newList;
    }
}