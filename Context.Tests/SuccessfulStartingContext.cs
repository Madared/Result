using ResultAndOption.Errors;
using ResultAndOption.Results;

namespace Context.Tests;

public static class SuccessfulContext
{
    public static Func<Result<string>> ResultFunc = () => Result<string>.Ok("hello");
    public static IContextResult<string> Context = ResultFunc.RunAndGetContext();
}

public class SuccessfulStartingContext
{
    [Fact]
    public void Is_Success()
    {
        Assert.True(SuccessfulContext.Context.Succeeded);
    }

    [Fact]
    public void Do_Mutates()
    {
        string? other = null;
        IContextResult<string> afterDo = SuccessfulContext.Context
            .Do(() => other = "new");
        Assert.True(afterDo.Succeeded);
        Assert.NotNull(other);
    }

    [Fact]
    public void Do_With_Input_Mutates()
    {
        string? other = null;
        IContextResult<string> afterDo = SuccessfulContext.Context
            .Do(str => other = str);
        Assert.True(afterDo.Succeeded);
        Assert.NotNull(other);
        Assert.Equal(SuccessfulContext.Context.Data, other);
    }

    [Fact]
    public void Do_Of_Result_Mutates()
    {
        string? other = null;
        IContextResult<string> afterDo = SuccessfulContext.Context
            .Do(() =>
            {
                other = "new";
                return Result.Ok();
            });
        Assert.True(afterDo.Succeeded);
        Assert.NotNull(other);
    }

    [Fact]
    public void Do_Of_Result_With_Input_Mutates()
    {
        string? other = null;
        IContextResult<string> afterDo = SuccessfulContext.Context
            .Do(str =>
            {
                other = str;
                return Result.Ok();
            });
        Assert.True(afterDo.Succeeded);
        Assert.Equal(SuccessfulContext.Context.Data, other);
        Assert.NotNull(other);
    }

    [Fact]
    public void Map_Of_Success_Gives_Success_Context()
    {
        IContextResult<int> mapped = SuccessfulContext.Context
            .Map(str => Result<int>.Ok(str.Length));

        Assert.True(mapped.Succeeded);
    }

    [Fact]
    public void Map_Of_Failure_Gives_Failed_Context()
    {
        IContextResult<int> mapped = SuccessfulContext.Context
            .Map(str => Result<int>.Fail(new UnknownError()));

        Assert.True(mapped.Failed);
    }

    [Fact]
    public void Map_Of_Func_Gives_Success_Context()
    {
        IContextResult<int> mapped = SuccessfulContext.Context
            .Map(str => str.Length);
        Assert.True(mapped.Succeeded);
    }

    [Fact]
    public void Retry_Returns_Same_Context()
    {
        IContextResult<string> retried = SuccessfulContext.Context.Retry();
        Assert.Equal(retried, SuccessfulContext.Context);
    }
}