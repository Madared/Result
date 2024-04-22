using ResultAndOption.Errors;

namespace ResultTests;

public class ExtensionTests {
    [Fact]
    public void ToResult_Returns_Failed_In_null_object() {
        //Given
        string? nullString = null;
        //When
        Result<string> stringResult = nullString.ToResult(new UnknownError());
        //Then
        Assert.True(stringResult.Failed);
    }

    [Fact]
    public void ToResult_Returns_ResultList_With_Correctly_Mapped_Values() {
        //Given
        Result<string> okResult = Result<string>.Ok("hello");
        Result<string> failedResult = Result<string>.Fail(new UnknownError());
        List<Result<string>> stringResults = new();
        stringResults.Add(okResult);
        stringResults.Add(failedResult);
        //When
        ResultList<string> resultList = stringResults.ToResultList();
        //Then
        Assert.True(resultList.HasErrors());
        Assert.Single(resultList.Errors);
        Assert.Single(resultList.Successes);
    }

    [Fact]
    public void ToResult_On_Option_Maps_Correctly() {
        IError error = new UnknownError();
        string hello = "hello";
        Option<string> none = Option<string>.None();
        Option<string> some = Option<string>.Some(hello);
        Result<string> noneResult = none.ToResult(error);
        Result<string> someResult = some.ToResult(error);
        Assert.True(noneResult.Failed);
        Assert.True(someResult.Succeeded);
        Assert.Equal(error, noneResult.Error);
        Assert.Equal(hello, someResult.Data);
    }
}