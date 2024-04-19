namespace Results;

public sealed class ResultSubscriber<T> where T : notnull  {
    public Result<T> Result { get; private set; }
    public ResultSubscriber(Result<T> result) {
        Result = result;
    }
    public void Update(Result<T> result) {
        Result = result;
    }
}