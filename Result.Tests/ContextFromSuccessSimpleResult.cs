namespace ResultTests;

public class SuccessSimpleStartingContext {
    private static Func<Result> resultFunc = Result.Ok;
    private static IContextResult context = resultFunc.RunAndGetContext();
    
    [Fact]
    public void Context_Is_Success
}