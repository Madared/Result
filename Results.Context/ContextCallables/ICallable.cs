using ResultAndOption.Results;
using ResultAndOption.Results.GenericResultExtensions;

namespace Results.Context.ContextCallables;

public interface ICallable
{
    Result Call();
}

public interface ICallable<TOut> : ICallable where TOut : notnull
{
    Result ICallable.Call()
    {
        return Call().ToSimpleResult();
    }

    Result<TOut> Call();
}