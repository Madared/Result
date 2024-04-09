namespace ResultTests;

public class FromSuccessSimpleResult {
    private static readonly Func<Result> resultFunc = Result.Ok;
    private static readonly IContextResult context = resultFunc.RunAndGetContext();

    [Fact]
    public void Context_Is_Success() {
        Assert.True(context.Succeeded);
    }

    [Fact]
    public void Mapping_With_Successful_Simple_Result_Function_Gives_Successful_Context() {
        IContextResult mapped = context.Map(Result.Ok);
        Assert.True(mapped.Succeeded);
    }

    [Fact]
    public void Mapping_With_Successful_Complex_Result_Function_Gives_Successful_Context() {
        IContextResult<string> mapped = context.Map(() => Result<string>.Ok("success"));
        Assert.True(mapped.Succeeded);
    }

    [Fact]
    public void Mapping_With_Action_Gives_Successful_Context() {
        IContextResult mapped = context.Map(() => Console.WriteLine("hello"));
        Assert.True(mapped.Succeeded);
    }

    [Fact]
    public void Mapping_With_Not_Null_Function_Gives_Successful_Context() {
        IContextResult<string> mapped = context.Map(() => "hello");
        Assert.True(mapped.Succeeded);
    }

    [Fact]
    public void Mapping_With_Failed_Simple_Result_Function_Gives_Failed_Context() {
        IContextResult mapped = context.Map(() => Result.Fail(new UnknownError()));
        Assert.True(mapped.Failed);
    }

    [Fact]
    public void Mapping_With_Failed_Complex_Result_Function_Gives_Failed_Context() {
        IContextResult<string> mapped = context.Map(() => Result<string>.Fail(new UnknownError()));
        Assert.True(mapped.Failed);
    }

    [Fact]
    public void Stripping_Context_Gives_Same_Result() {
        Result stripped = context.StripContext();
        Assert.Equal(resultFunc(), stripped);
    }

    [Fact]
    public void Retry_Returns_Same_Context() {
        IContextResult retried = context.Retry();
        Assert.Equal(context, retried);
    }
}