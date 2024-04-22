using ResultAndOption.Results;

namespace Context.ContextResults;

public sealed class ResultSubscriber<T> where T : notnull {
    public ResultSubscriber(Result<T> result) {
        Result = result;
    }

    public Result<T> Result { get; private set; }

    public void Update(Result<T> result) {
        Result = result;
    }
}