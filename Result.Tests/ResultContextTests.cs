namespace ResultTests;

public class ResultContextTests {
    
    [Fact]
    public void Result_Can_Retry_Function_That_Created_It() {
        CallableThing callableThing = new("I have been called");
        ContextResult<string, string> contextResult = ContextResult<string, string>.FromCallable(() => callableThing.Call().ToResult(new UnknownError()));
        ContextResult<string, string> retried = contextResult.Retry();
        Assert.Equal(1, callableThing.TimesCalled);
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
}

public class CallableDataThing {
    public CallableDataThing(string message) {
        Message = message;
    }
    public int TimesCalled { get; private set; } = 0;
    public string Message { get; }

    public string Call(string value) => string.Concat(value, Message);
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