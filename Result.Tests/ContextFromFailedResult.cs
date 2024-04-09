namespace ResultTests;

public class ContextFromFailedResult {
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
}