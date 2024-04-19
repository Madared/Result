namespace ResultTests;

public class MultipleRetries {
    private static readonly Func<Result<string>> ResultFunc = () => Result<string>.Ok("hello");
    private static readonly IContextResult<string> Context = ResultFunc.RunAndGetContext();

    [Fact]
    public void Multiple_Retries_Calls_Callable_Until_Success_Or_Retries_Demanded() {
        MultipleRetryable retryable = new(3);
        IContextResult<string> retried = Context
            .Do(() => retryable.Mutate())
            .Retry(5);

        Assert.True(retried.Succeeded);
        Assert.Equal(retryable.TimesRun, retryable.SuccessOn);
    }

    [Fact]
    public void Multiple_Retries_On_Context_Result_Calls_Until_Success_Or_Retries_Demanded() {
        MultipleRetryable retryable = new(3);
        IContextResult<string> retried = Context
            .Map(str => str.Length)
            .Do(() => retryable.Mutate())
            .Map(num => num.ToString())
            .Retry(3);

        Assert.True(retried.Succeeded);
        Assert.Equal(retryable.TimesRun, retryable.SuccessOn);
    }
}