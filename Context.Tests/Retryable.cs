using ResultAndOption.Errors;
using ResultAndOption.Results;

namespace Context.Tests;

public class Retryable {
    private bool _called;
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