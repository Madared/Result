namespace ResultTests;

public static class FailureContext {
    public static readonly Func<Result<string>> ResultFunc = () => Result<string>.Fail(new UnknownError());
    public static readonly IContextResult<string> Context = ResultFunc.RunAndGetContext();
}

public class MapChainFromFailedContext {
    private static readonly Func<Result<string>> ResultFunc = () => Result<string>.Fail(new UnknownError());
    private static readonly IContextResult<string> Context = ResultFunc.RunAndGetContext();

    [Fact]
    public void Retry_Does_Not_Change_Final_Result() {
        IContextResult<string> mapped = Context
            .Map(str => str.Length)
            .Map(length => length.ToString())
            .Map(str => str.ToLower());

        Assert.True(mapped.Failed);

        IContextResult<string> retried = mapped.Retry();
        Assert.Equal(mapped.Failed, retried.Failed);
        Assert.Equal(mapped.Error.GetType(), retried.Error.GetType());
    }

    [Fact]
    public void Error_Gets_Passed_Down_The_Context() {
        IContextResult<string> mapped = Context
            .Map(str => str.Length)
            .Map(length => length.ToString())
            .Map(str => str.ToLower());

        Assert.True(mapped.Failed);
        Assert.Equal(Context.Error.GetType(), mapped.Error.GetType());
    }
}