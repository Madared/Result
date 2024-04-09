namespace ResultTests;

public class ContextMapChainFromSuccessContext {
    private static readonly Func<Result<string>> ResultFunc = () => Result<string>.Ok("Start");
    private static readonly IContextResult<string> Context = ResultFunc.RunAndGetContext();

    [Fact]
    public void Mapping_With_Only_Successes_Gives_Success() {
        IContextResult<int> mapped = Context
            .Map(str => str.Length)
            .Map(i => Result<int>.Ok(i * i))
            .Map(i => i.ToString())
            .Map(str => str.Length);
        
        Assert.True(mapped.Succeeded);
        Assert.Equal(2, mapped.Data);
    }

    [Fact]
    public void Retrying_Retryable_Will_Give_Success_Context () {
        string name = "Name";
        Retryable retryable = new();
        IContextResult<string> mapped = Context
            .Map(() => name)
            .Map(retryable.AddHello);
        
        Assert.True(mapped.Failed);

        IContextResult<string> retried = mapped.Retry();
        Assert.True(retried.Succeeded);
        Assert.Equal(name + retryable.Hello, retried.Data);
    }

    [Fact]
    public void Successful_Side_Effects_Dont_Rerun_On_Retry() {
        int timesCalled = 0;
        Retryable retryable = new();
        IContextResult<string> mapped = Context
            .Map(() => timesCalled++)
            .Map(() => "Name")
            .Map(name => retryable.AddHello(name))
            .Retry();
        
        Assert.Equal(1, timesCalled);
    }
}