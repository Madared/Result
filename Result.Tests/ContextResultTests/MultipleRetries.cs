namespace ResultTests;

public class MultipleRetries {
    private static Func<Result<string>> ResultFunc = () => Result<string>.Ok("hello");
    private static IContextResult<string> Context = ResultFunc.RunAndGetContext();

    [Fact]
    public void Multiple_Retries_Calls_Callable_Until_Success_Or_Retries_Demanded() {
        MultipleRetryable retryable = new(3);
        IContextResult retried = Context
            .Map(() => retryable.Mutate())
            .Retry(5);
        
        Assert.True(retried.Succeeded);
        Assert.Equal(retryable.TimesRun, retryable.SuccessOn);
    }
}