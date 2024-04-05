using System.Runtime.InteropServices;

namespace ResultTests;

public class ResultContextTests {
    
    [Fact]
    public void Result_Can_Retry_Function_That_Created_It() {
        CallableThing callableThing = new("I have been called");
        //Gets called once here
        ContextResult<string, string> contextResult = ContextResult<string, string>.FromCallable(() => callableThing.Call().ToResult(new UnknownError()));
        //Gets called again here
        ContextResult<string, string> retried = contextResult.Retry();
        Assert.Equal(2, callableThing.TimesCalled);
        Assert.Equal(callableThing.Message, contextResult.Data);
    }

    [Fact]
    public void Retry_Goes_Up_The_Chain() {
        CallableThing callableThing = new("First call");
        CallableThing callableThing2 = new("Second call");
        ContextResult<string, string> contextResult = ContextResult<string, string>.FromCallable(() => callableThing.Call().ToResult(new UnknownError()));
        ContextResult<string, string> contextResult2 = ContextResult<string, string>.FromCallable(() => callableThing2.Call().ToResult(new UnknownError()));
        contextResult2.Retry();
        Assert.Equal(callableThing2.Message, contextResult2.Data);
        Assert.Equal(callableThing.Message, contextResult.Data);
    }

    [Fact]
    public void Default_ContextResult_Is_Failure() {
        ContextResult<string, string> r = default;
        Assert.True(r.Failed);
    }

    [Fact]
    public void Default_Retry_Maintains_Failure_Result() {
        ContextResult<string, string> r = default;
        ContextResult<string, string> retried = r.Retry();
        Assert.True(retried.Failed);
    }

    [Fact]
    public void Map_On_Success_ContextResult_With_Successful_Callable_Returns_Success() {
        ContextResult<string, string> success = ContextResult<string, string>.Ok("hello");
        ContextResult<string, int> mapped = success.Map(s => (s.Length).ToResult(new UnknownError()));
        Assert.True(mapped.Succeeded);
    }

    [Fact]
    public void Mapping_Successful_Result_With_Non_Result_Callable_Returns_Success() {
        ContextResult<string, string> success = ContextResult<string, string>.Ok("hello");
        ContextResult<string, int> mapped = success.Map(s => s.Length);
        Assert.True(mapped.Succeeded);
    }

    [Fact]
    public void Mapping_Failed_Result_With_Successful_Non_Result_Callable_Returns_Failure() {
        ContextResult<string, string> failed = ContextResult<string, string>.Fail(new ContextResultCallableNoArguments<string>(() => Result<string>.Fail(new UnknownError())), new UnknownError());
        ContextResult<string, int> mapped = failed.Map(s => s.Length);
        Assert.True(mapped.Failed);
    }

    [Fact]
    public void Mapping_Failed_Result_With_Successful_Result_Callable_Returns_Failure() {
        ContextResult<string, string> failed = ContextResult<string, string>.Fail(new ContextResultCallableNoArguments<string>(() => Result<string>.Fail(new UnknownError())), new UnknownError());
        ContextResult<string, int> mapped = failed.Map(s => s.Length.ToResult(new UnknownError()));
        Assert.True(mapped.Failed);
    }

    [Fact]
    public void Retrying_Mapped_Result_From_Failed_Result_Recalls_Original_Callable() {
        int timesCalled = 0;
        ContextResult<int, int> failed = ContextResult<int, int>.Fail(new ContextResultCallableNoArguments<int>(() => {
            timesCalled++;
            return timesCalled.ToResult(new UnknownError());
        }), new UnknownError());

        ContextResult<int, int> mapped = failed.Map(total => total);
        ContextResult<int, int> retried = mapped.Retry();
        Assert.Equal(1, timesCalled);
    }
}
public class CallableThing {
    public CallableThing(string message) {
        Message = message;
    }
    public int TimesCalled { get; private set; } = 0;
    public string Message { get; }

    public string Call() {
        TimesCalled++;
        return Message;
    }
}