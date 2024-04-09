namespace ResultTests;

public class Retryable {
    private bool _called = false;
    public string Hello = " Hello.";

    public Result<string> AddHello(string name) {
        if (!_called) {
            _called = true;
            return Result<string>.Fail(new UnknownError());
        }
        string added = name + Hello;
        return Result<string>.Ok(added);
    }
}