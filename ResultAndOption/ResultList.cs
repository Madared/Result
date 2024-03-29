namespace Results;

public class ResultList<T> where T : notnull {
    public ResultList() {
        Successes = new List<T>();
        Errors = new List<IError>();
    }

    private ResultList(List<IError> errors) {
        Successes = new List<T>();
        Errors = errors;
    }

    public List<T> Successes { get; }
    public List<IError> Errors { get; }

    public void AddResult(Result<T> result) {
        if (result.Failed)
            Errors.Add(result.Error);
        else
            Successes.Add(result.Data);
    }

    public void AddResults(IEnumerable<Result<T>> results) {
        foreach (Result<T> result in results) AddResult(result);
    }

    public bool HasErrors() {
        return Errors.Count > 0;
    }

    public ResultList<TOut> Map<TOut>(Func<T, Result<TOut>> function) where TOut : notnull {
        return HasErrors()
            ? new ResultList<TOut>(Errors)
            : Successes
                .Select(function)
                .ToResult();
    }

    public ResultList<TOut> Map<TOut>(Func<T, TOut?> function, IError nullabilityError) where TOut : notnull {
        return HasErrors()
            ? new ResultList<TOut>(Errors)
            : Successes
                .Select(data => function(data).ToResult(nullabilityError))
                .ToResult();
    }

    public Result<TOut> MapList<TOut>(Func<IEnumerable<T>, TOut?> function, IError nullabilityError)
        where TOut : notnull {
        return HasErrors()
            ? Result<TOut>.Fail(new MultipleErrors(Errors))
            : function(Successes).ToResult(nullabilityError);
    }

    public ResultList<TOut> MapList<TOut>(Func<IEnumerable<T>, IEnumerable<Result<TOut>>> function)
        where TOut : notnull {
        return HasErrors()
            ? new ResultList<TOut>(Errors)
            : function(Successes).ToResult();
    }

    public Result<TOut> MapList<TOut>(Func<IEnumerable<T>, TOut> function) where TOut : notnull {
        return HasErrors()
            ? Result<TOut>.Fail(new MultipleErrors(Errors))
            : Result<TOut>.Ok(function(Successes));
    }

    public Result<TOut> MapList<TOut>(Func<IEnumerable<T>, Result<TOut>> function) where TOut : notnull {
        return HasErrors()
            ? Result<TOut>.Fail(new MultipleErrors(Errors))
            : function(Successes);
    }
}