using ResultAndOption;
using ResultAndOption.Results;

namespace Results.Context.ContextResults;

public sealed class ResultEmitter<T> where T : notnull
{
    private readonly List<ResultSubscriber<T>> _subscribers;

    public ResultEmitter()
    {
        _subscribers = new List<ResultSubscriber<T>>();
    }

    public void Emit(Result<T> result)
    {
        foreach (ResultSubscriber<T> subscriber in _subscribers) subscriber.Update(result);
    }

    public void Subscribe(ResultSubscriber<T> subscriber)
    {
        _subscribers.Add(subscriber);
    }
}