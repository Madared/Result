namespace ResultTests;

public class SuccessfulContextResult {
    private static IContextResult<string> Context = SuccessfulContext.Context.Map(str => str);

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
}