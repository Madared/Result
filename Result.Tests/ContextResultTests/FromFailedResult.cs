namespace ResultTests;

public class FromFailedResult {
    private static readonly Func<Result<string>> ResultFunc = () => Result<string>.Fail(new UnknownError());
    private static readonly IContextResult<string> Context = ResultFunc.RunAndGetContext();

    [Fact]
    public void Context_Is_Failure() {
        Assert.True(Context.Failed);
    }

    [Fact]
    public void Mapping_With_Action_Gives_Failed_Context() {
        IContextResult mapped = Context.Map(() => Console.WriteLine("hello"));
        Assert.True(mapped.Failed);
    }

    [Fact]
    public void Mapping_With_Success_Simple_Result_Function_Gives_Failed_Context() {
        IContextResult mapped = Context.Map(Result.Ok);
        Assert.True(mapped.Failed);
    }

    [Fact]
    public void Mapping_With_Not_Null_Function_Gives_Failed_Result() {
        IContextResult<string> mapped = Context.Map(() => "hello");
        Assert.True(mapped.Failed);
    }

    [Fact]
    public void Mapping_With_Success_Complex_Result_Function_Gives_Failed_Result() {
        IContextResult<string> mapped = Context.Map(() => Result<string>.Ok("hello"));
        Assert.True(mapped.Failed);
    }

    [Fact]
    public void Stripping_Context_Gives_Same_Result() {
        Result<string> original = ResultFunc();
        Result<string> stripped = Context.StripContext();
        Assert.Equal(original.Failed, stripped.Failed);
        Assert.Equal(original.Error.Message, stripped.Error.Message);
        Assert.Equal(original.GetType(), stripped.GetType());
    }

    [Fact]
    public void Retrying_Multiple_Times_On_Failure_Keeps_Failure() {
        int timesCalled = 0;
        IContextResult retried = Context
            .Map(() => timesCalled++)
            .Retry(3);
        
        Assert.True(retried.Failed);
        Assert.Equal(0, timesCalled);
    }
}

public class MultipleRetryable {
    public int TimesRun { get; private set; }
    public const int SuccessOn = 3;
    public Result Mutate() => TimesRun < SuccessOn ? Result.Fail(new UnknownError()) : Result.Ok();
}