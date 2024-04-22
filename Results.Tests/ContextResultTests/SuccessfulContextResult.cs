using ResultAndOption.Errors;
using ResultAndOption.Results;
using ResultTests;

namespace Results.Tests.ContextResultTests;

public class SuccessfulContextResult {
    private static readonly IContextResult<string> Context = SuccessfulContext.Context.Map(str => str);

    [Fact]
    public void Is_Success() {
        Assert.True(Context.Succeeded);
    }

    [Fact]
    public void Do_Mutates() {
        string? other = null;
        IContextResult<string> afterDo = Context
            .Do(str => other = str);
        Assert.True(afterDo.Succeeded);
        Assert.NotNull(other);
        Assert.Equal(Context.Data, other);
    }

    [Fact]
    public void Do_Of_Success_Result_Mutates() {
        string? other = null;
        IContextResult<string> afterDo = Context
            .Do(str => {
                other = str;
                return Result.Ok();
            });
        Assert.True(afterDo.Succeeded);
        Assert.NotNull(other);
        Assert.Equal(Context.Data, other);
    }

    [Fact]
    public void Successful_Dos_Dont_Rerun_On_Retry() {
        int changed = 0;
        IContextResult<string> retried = Context
            .Do(() => changed++)
            .Map(str => Result<string>.Fail(new UnknownError()))
            .Retry();

        Assert.Equal(1, changed);
        Assert.True(retried.Failed);
    }

    [Fact]
    public void Multiple_Retries_Go_To_Success_Or_Times_Asked() {
        int timesToSuccess = 3;
        MultipleRetryable retryable = new(timesToSuccess);
        IContextResult<string> retried = Context
            .Do(() => retryable.Mutate())
            .Retry(timesToSuccess);

        Assert.True(retried.Succeeded);
        Assert.Equal(timesToSuccess, retryable.TimesRun);
    }

    [Fact]
    public void False_Error_Predicate_Does_Not_Retry() {
        int timesToSuccess = 3;
        MultipleRetryable retryable = new(timesToSuccess);
        IContextResult<string> retried = Context
            .Do(() => retryable.Mutate())
            .Retry(timesToSuccess, error => error is MultipleErrors);

        Assert.True(retried.Failed);
        Assert.Equal(1, retryable.TimesRun);
    }
}