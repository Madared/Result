namespace ResultTests;

public class FailedContextResult {
    private static readonly IContextResult<string> Context = FailureContext.Context.Map(str => str);

    [Fact]
    public void Do_Of_Action_Gives_Failed_Context() {
        string? other = null;
        IContextResult<string> afterDo = Context.Do(() => other = "hello");
        Assert.True(afterDo.Failed);
        Assert.Null(other);
    }

    [Fact]
    public void Do_Of_Input_Action_Gives_Failed_Result() {
        string? other = null;
        IContextResult<string> afterDo = Context.Do(str => other = null);
        Assert.True(afterDo.Failed);
        Assert.Null(other);
    }

    [Fact]
    public void Do_Of_Result_Gives_Failed_Result() {
        string? other = null;
        IContextResult<string> afterDo = Context.Do(() => {
            other = "hello";
            return Result.Ok();
        });
        Assert.True(afterDo.Failed);
        Assert.Null(other);
    }

    [Fact]
    public void Do_Of_Result_With_Input_Gives_Failed_Result() {
        string? other = null;
        IContextResult<string> afterDo = Context.Do(str => {
            other = str;
            return Result.Ok();
        });
        Assert.True(afterDo.Failed);
        Assert.Null(other);
    }

    [Fact]
    public void Map_With_Input_Gives_Failed_Result() {
        IContextResult<int> mapped = Context.Map(str => str.Length);
        Assert.True(mapped.Failed);
    }

    [Fact]
    public void Map_Of_Result_With_Input_Gives_Failed_Result() {
        IContextResult<int> mapped = Context.Map(str => Result<int>.Ok(str.Length));
        Assert.True(mapped.Failed);
    }

    [Fact]
    public void Map_Of_Result_Gives_Failed_Result() {
        IContextResult<int> mapped = Context.Map(() => Result<int>.Ok(100));
        Assert.True(mapped.Failed);
    }

    [Fact]
    public void Map_Without_Input_Gives_Failed_Result() {
        IContextResult<int> mapped = Context.Map(() => 100);
        Assert.True(mapped.Failed);
    }
}