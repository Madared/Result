using ResultAndOption.Errors;
using ResultAndOption.Results;

namespace Results.Tests.ContextResultTests;

public class FromFailedSimpleResult {
    private static readonly Result Failed = Result.Fail(new UnknownError());
    private static readonly Func<Result> ResultFunc = () => Failed;
    private static readonly IContextResult Context = ResultFunc.RunAndGetContext();

    [Fact]
    public void Context_Is_Failure() {
        Assert.True(Context.Failed);
    }

    [Fact]
    public void Mapping_With_Successful_Result_Function_Gives_Failed_Context_With_Same_Error() {
        var mapped = Context.Do(Result.Ok);
        Assert.True(mapped.Failed);
        Assert.Equal(Context.Error, mapped.Error);
    }

    [Fact]
    public void Mapping_With_Successful_Complex_Result_Function_Gives_Failed_Context_With_Same_Error() {
        IContextResult<string> mapped = Context.Map(() => Result<string>.Ok("hello"));
        Assert.True(mapped.Failed);
        Assert.Equal(Context.Error, mapped.Error);
    }

    [Fact]
    public void Mapping_With_Action_Gives_Failed_Context_Doesnt_Run_The_Action_And_Keeps_Same_Error() {
        bool called = false;
        IContextResult mapped = Context.Map(() => called = true);
        Assert.True(mapped.Failed);
        Assert.False(called);
        Assert.Equal(Context.Error, mapped.Error);
    }

    [Fact]
    public void Mapping_With_Not_Null_Function_Gives_Failed_Context_And_Same_Error() {
        IContextResult<string> mapped = Context.Map(() => "Hello");
        Assert.True(mapped.Failed);
        Assert.Equal(Context.Error, mapped.Error);
    }

    [Fact]
    public void Stripping_Context_Gives_Same_Result() {
        var stripped = Context.StripContext();
        Assert.Equal(ResultFunc(), stripped);
    }
}