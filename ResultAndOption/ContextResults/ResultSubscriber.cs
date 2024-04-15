namespace Results;

public class ResultSubscriber<T> where T : notnull  {
    public Result<T> Result { get; private set; }
    public ResultSubscriber(Result<T> result) {
        Result = result;
    }
    public void Notify(Result<T> result) {
        Result = result;
    }
}