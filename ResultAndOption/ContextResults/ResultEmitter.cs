namespace Results;

public class ResultEmitter<T> where T : notnull {
    private readonly List<ResultSubscriber<T>> _subscribers;
    public ResultEmitter() {
        _subscribers = new List<ResultSubscriber<T>>();
    }

    public void SetResult(Result<T> result) {
        _subscribers.ForEach(sub => sub.Notify(result));
    }
    public void Subscribe(ResultSubscriber<T> subscriber) {
        _subscribers.Add(subscriber);
    }
}