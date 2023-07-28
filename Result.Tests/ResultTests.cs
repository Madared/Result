namespace ResultTests;

public class ResultTests
{
    [Fact]
    public void Failure_Checking_Returns_Correct_Value()
    {
        //Given
        Result<string> stringResult = Result<string>.Fail(new UnknownError());

        //When
        bool failed = stringResult.Failed;
        bool succeeded = stringResult.Succeeded;
        //Then
        Assert.True(failed);
        Assert.False(succeeded);
    }

    [Fact]
    public void Success_Checking_Returns_Correct_Values()
    {
        //Given
        Result<string> stringResult = Result<string>.Ok("hello");
        //When
        bool failed = stringResult.Failed;
        bool succeeded = stringResult.Succeeded;
        //Then
        Assert.True(succeeded);
        Assert.False(failed);
    }

    [Fact]
    public void Accessing_Data_In_Failed_Result_Throws_InvalidOperationException()
    {
        //Given
        Result<string> stringResult = Result<string>.Fail(new UnknownError());
        //Then
        Assert.Throws<InvalidOperationException>(() => stringResult.Data);
    }

    [Fact]
    public void Accessing_Data_In_Failed_Result_Of_Non_Nullable_Value_Type_Throws()
    {
        //Given
        Result<int> intResult = Result<int>.Fail(new UnknownError());
        //Then
        Assert.Throws<InvalidOperationException>(() => intResult.Data);
    }

    [Fact]
    public void Successfull_Result_IfSucceeded_Completes_Action_And_Returns_Self()
    {
        //Given
        Result<string> stringResult = Result<string>.Ok("world");
        //When
        string helloWorld = "hello";
        Result<string> afterActionResult = stringResult
            .IfSucceeded(data => helloWorld += $" {data.Data}");
        //Then
        Assert.Equal(stringResult, afterActionResult);
        Assert.Equal("hello world", helloWorld);
    }

    [Fact]
    public void Failure_Result_IfFailed_Completes_Action_And_Returns_Self()
    {
        //Given
        Result<string> stringResult = Result<string>.Fail(new UnknownError());
        //When
        string errorMessage = "";
        Result<string> afterActionResult = stringResult
            .IfFailed(result => errorMessage = result.Error.Message);
        //Then
        Assert.Equal(stringResult, afterActionResult);
        Assert.Equal(errorMessage, stringResult.Error.Message);
    }

    [Fact]
    public void ErrorConversion_On_Failed_Transfers_Correctly()
    {
        //Given
        Result<string> stringResult = Result<string>.Fail(new UnknownError());
        //When
        Result<int> intResult = stringResult.ConvertErrorResult<int>();
        //Then
        Assert.IsType<Result<int>>(intResult);
        Assert.Equal(stringResult.Error, intResult.Error);
    }

    [Fact]
    public void ErrorConversion_On_Success_Throws_InvalidOperationException()
    {
        //Given
        Result<string> stringResult = Result<string>.Ok("hello");
        //Then
        Assert.Throws<InvalidOperationException>(() => stringResult.ConvertErrorResult<int>());
    }

    [Fact]
    public void UseData_On_Result_Returning_Functions_Returns_Correct_Response()
    {
        //Given
        string helloWorld = "hello";
        Result GetSuccess(string world) { helloWorld += $" {world}"; return Result.Ok(); }
        Result GetFailure() => Result.Fail(new UnknownError());
        Result<string> stringResult = Result<string>.Ok("world");
        //When
        Result<string> postGettingSuccess = stringResult.UseData(data => GetSuccess(data));
        Result<string> postGettingFailure = stringResult.UseData(data => GetFailure());
        //Then
        Assert.True(postGettingSuccess.Succeeded);
        Assert.True(postGettingFailure.Failed);
        Assert.Equal("hello world", helloWorld);
    }

    [Fact]
    public void Mapping_Failed_Result_Returns_A_Failed_Result_And_Passes_The_Error()
    {
        //Given
        Result<string> stringResult = Result<string>.Fail(new UnknownError());
        //When
        Result<int> mappedResult = stringResult
            .Map(data => data.Count());
        //Then
        Assert.True(mappedResult.Failed);
        Assert.Equal(stringResult.Error, mappedResult.Error);
    }

    [Fact]
    public void Mapping_Successful_Result_Returns_Correct_Data_In_Result_Object()
    {
        //Given
        Result<string> stringResult = Result<string>.Ok("hello");
        //When
        Result<int> mappedResult = stringResult
            .Map(data => data.Count());
        //Then
        Assert.Equal(5, mappedResult.Data);
    }

    [Fact]
    public void ToSimpleResult_Correctly_Maps()
    {
        //Given
        Result<string> okStringResult = Result<string>.Ok("hello");
        Result<string> failedStringResult = Result<string>.Fail(new UnknownError());
        //When
        Result simpleOkResult = okStringResult.ToSimpleResult();
        Result simpleFailedResult = failedStringResult.ToSimpleResult();
        //Then
        Assert.True(simpleOkResult.Succeeded);
        Assert.True(simpleFailedResult.Failed);
        Assert.Equal(failedStringResult.Error, simpleFailedResult.Error);
    }
}