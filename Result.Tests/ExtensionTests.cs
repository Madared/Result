namespace ResultTests;

public class ExtensionTests
{
    [Fact]
    public void ToResult_Returns_Failed_In_null_object()
    {
        //Given
        string? nullString = null;
        //When
        Result<string> stringResult = nullString.ToResult(new UnknownError());
        //Then
        Assert.True(stringResult.Failed);
    }

    [Fact]
    public void ToResult_Returns_ResultList_With_Correctly_Mapped_Values()
    {
        //Given
        Result<string> okResult = Result<string>.Ok("hello");
        Result<string> failedResult = Result<string>.Fail(new UnknownError());
        List<Result<string>> stringResults = new();
        stringResults.Add(okResult);
        stringResults.Add(failedResult);
        //When
        ResultList<string> resultList = stringResults.ToResult();
        //Then
        Assert.True(resultList.HasErrors());
        Assert.Single(resultList.Errors);
        Assert.Single(resultList.Successes);
    }

    [Fact]
    public void ListMap_Correctly_Maps_Every_Value()
    {
        //Given
        List<int> expectedValues = new()
        {
            2, 3, 4, 5, 6
        };
        List<int> values = new()
        {
            1, 2, 3, 4, 5
        };
        //When
        List<int> newValues = values.ListMap(value => value + 1);
        //Then
        Assert.Equal(expectedValues, newValues);
    }

    [Fact]
    public void LisMap_Handles_Null()
    {
        //Given
        List<string> expectedValues = new()
        {
            "hello world",
            "hello world",
            " world",
            "hello world"
        };
        List<string?> values = new()
        {
            "hello",
            "hello",
            null,
            "hello"
        };
        //When
        List<string> newValues = values.ListMap(value => value + " world");
        //Then
        Assert.Equal(expectedValues, newValues);
    }
}