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
    public void Uses_Previous_Calls_Data_On_Retry() {
        CallableDataThing callableDataThing = new("my data");
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