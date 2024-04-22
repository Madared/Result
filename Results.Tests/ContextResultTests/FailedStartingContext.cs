using ResultAndOption.Errors;
using ResultAndOption.Results;

namespace Results.Tests.ContextResultTests;

public class FailedStartingContext {
    [Fact]
    public void Mapping_Gives_Failed_Context() {
        IContextResult<int> mapped = FailureContext.Context.Map(str => str.Length);
        Assert.True(mapped.Failed);
    }

    [Fact]
    public void Do_Of_Input_Action_Gives_Failed_Context() {
        string? otherHello = null;
        IContextResult<string> afterDo = FailureContext.Context.Do(str => otherHello = str);
        Assert.True(afterDo.Failed);
        Assert.Null(otherHello);
    }

    [Fact]
    public void Do_Of_Result_Func_Gives_Failed_Context() {
        string? otherHello = null;
        IContextResult<string> afterDo = FailureContext.Context.Do(str => {
            otherHello = str;
            return Result.Ok();
        });

        IContextResult<string> afterDoFailure = FailureContext.Context.Do(str => {
            otherHello = str;
            return Result.Fail(new UnknownError());
        });

        Assert.True(afterDo.Failed);
        Assert.True(afterDoFailure.Failed);
        Assert.Null(otherHello);
    }

    [Fact]
    public void Do_Of_Action_Gives_Failed_Context() {
        string? other = null;
        IContextResult<string> afterDo = FailureContext.Context.Do(() => other = "new");
        Assert.True(afterDo.Failed);
        Assert.Null(other);
    }

    [Fact]
    public void Map_Of_Func_Gives_Failed_Context() {
        IContextResult<int> mapped = FailureContext.Context.Map(str => str.Length);
        Assert.True(mapped.Failed);
    }

    [Fact]
    public void Map_Of_Result_Func_Gives_Failed_Context() {
        IContextResult<int> mapped = FailureContext.Context.Map(str => Result<int>.Ok(str.Length));
        IContextResult<int> mappedFailure = FailureContext.Context.Map(str => Result<int>.Fail(new UnknownError()));
        Assert.True(mapped.Failed);
        Assert.True(mappedFailure.Failed);
    }

    [Fact]
    public void Map_Of_No_Input_Func_Gives_Failed_Context() {
        IContextResult<int> mapped = FailureContext.Context.Map(() => 100);
        Assert.True(mapped.Failed);
    }

    [Fact]
    public void Map_Of_No_Input_Result_Func_Gives_Failed_Context() {
        IContextResult<int> mapped = FailureContext.Context.Map(() => Result<int>.Ok(100));
        Assert.True(mapped.Failed);
    }

    [Fact]
    public void Retry_Does_Not_Change_Result() {
        IContextResult<string> retried = FailureContext.Context.Retry();
        Assert.True(retried.Failed);
    }
}