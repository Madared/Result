namespace ResultTests;

public class ResultContextTests {
    
    [Fact]
    public void Result_Can_Retry_Function_That_Created_It() {
        CallableThing callableThing = new("I have been called");
        ContextedResult<string> contextedResult = new(callableThing.Call, "hello");
        ContextedResult<string> retried = contextedResult.Retry();
        Assert.Equal(1, callableThing.TimesCalled);
        Assert.Equal(callableThing.Message, contextedResult.Data);
    }

    [Fact]
    public void Retry_Goes_Up_The_Chain() {
        CallableThing callableThing = new("First call");
        CallableThing callableThing2 = new("Second call");
        ContextedResult<string> contextedResult = new(callableThing.Call, "starting data");
        ContextedResult<string> contextedResult2 = new(callableThing2.Call, null, contextedResult);
        contextedResult2.Retry();
        Assert.Equal(callableThing2.Message, contextedResult2.Data);
        Assert.Equal(callableThing.Message, contextedResult.Data);
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