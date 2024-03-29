namespace ResultTests;

public class ResultTests {
    [Fact]
    public void Failure_Checking_Returns_Correct_Value() {
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
    public void Success_Checking_Returns_Correct_Values() {
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
    public void Accessing_Data_In_Failed_Result_Throws_InvalidOperationException() {
        //Given
        Result<string> stringResult = Result<string>.Fail(new UnknownError());
        //Then
        Assert.Throws<InvalidOperationException>(() => stringResult.Data);
    }

    [Fact]
    public void Accessing_Data_In_Failed_Result_Of_Non_Nullable_Value_Type_Throws() {
        //Given
        Result<int> intResult = Result<int>.Fail(new UnknownError());
        //Then
        Assert.Throws<InvalidOperationException>(() => intResult.Data);
    }

    [Fact]
    public void Successfull_Result_IfSucceeded_Completes_Action_And_Returns_Self() {
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
    public void Failure_Result_IfFailed_Completes_Action_And_Returns_Self() {
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
    public void ErrorConversion_On_Failed_Transfers_Correctly() {
        //Given
        Result<string> stringResult = Result<string>.Fail(new UnknownError());
        //When
        Result<int> intResult = stringResult.ConvertErrorResult<int>();
        //Then
        Assert.IsType<Result<int>>(intResult);
        Assert.Equal(stringResult.Error, intResult.Error);
    }

    [Fact]
    public void ErrorConversion_On_Success_Throws_InvalidOperationException() {
        //Given
        Result<string> stringResult = Result<string>.Ok("hello");
        //Then
        Assert.Throws<InvalidOperationException>(() => stringResult.ConvertErrorResult<int>());
    }

    [Fact]
    public void UseData_On_Result_Returning_Functions_Returns_Correct_Response() {
        //Given
        string helloWorld = "hello";

        Result GetSuccess(string world) {
            helloWorld += $" {world}";
            return Result.Ok();
        }

        Result GetFailure() {
            return Result.Fail(new UnknownError());
        }

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
    public void Mapping_Failed_Result_Returns_A_Failed_Result_And_Passes_The_Error() {
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
    public void Mapping_Successful_Result_Returns_Correct_Data_In_Result_Object() {
        //Given
        Result<string> stringResult = Result<string>.Ok("hello");
        //When
        Result<int> mappedResult = stringResult
            .Map(data => data.Length);
        //Then
        Assert.Equal(5, mappedResult.Data);
    }

    [Fact]
    public void ToSimpleResult_Correctly_Maps() {
        //Given
        Result<string> okStringResult = Result<string>.Ok("hello");
        Result<string> failedStringResult = Result<string>.Fail(new UnknownError());
        //When
        var simpleOkResult = okStringResult.ToSimpleResult();
        var simpleFailedResult = failedStringResult.ToSimpleResult();
        //Then
        Assert.True(simpleOkResult.Succeeded);
        Assert.True(simpleFailedResult.Failed);
        Assert.Equal(failedStringResult.Error, simpleFailedResult.Error);
    }

    [Fact]
    public void Aggregation_With_Tuples() {
        Result<(string str, int number, string str2)> tupleResult = Result<string>.Ok("hello")
            .Map(str => (str, number: 10))
            .Map(data => (data.str, data.number, str2: "hello"));
        Assert.True(tupleResult.Succeeded);
        Assert.Equal("hello", tupleResult.Data.str);
        Assert.Equal(10, tupleResult.Data.number);
        Assert.Equal("hello", tupleResult.Data.str2);
    }

    [Fact]
    public void ToResult_Works_ToCreate_ResultList() {
        //Given
        List<Result<string>> stringResults = new() {
            Result<string>.Ok("hello"),
            Result<string>.Ok("world")
        };
        //When
        ResultList<string> resultList = stringResults.ToResult();
        //Then
        Assert.False(resultList.HasErrors());
        Assert.Equal(2, resultList.Successes.Count);
    }

    [Fact]
    public void Or_On_Failed_Result_Returns_Passed_In_Value() {
        Result<int> failed = Result<int>.Fail(new UnknownError());
        int fromOr = failed.Or(5);
        Assert.Equal(5, fromOr);
    }

    [Fact]
    public void Or_On_Successful_Result_Returns_Internal_Value() {
        Result<int> successful = Result<int>.Ok(100);
        int fromOr = successful.Or(5);
        Assert.Equal(100, fromOr);
    }

    [Fact]
    public void MapAsync_Maps_Internal_Result_Extracting_Task() {
        Result<string> stringResult = Result<string>.Ok("hello");
        Task<Result<string>> taskResult = Task.FromResult(stringResult);

        async Task<int> SomeAsyncFunc(string f) {
            return f.Length;
        }

        Task<Result<int>> intResultTask = taskResult.MapAsync(SomeAsyncFunc);
        Result<int> intResult = intResultTask.Result;
        Assert.True(intResult.Succeeded);
        Assert.Equal(stringResult.Data.Length, intResult.Data);
    }

    [Fact]
    public void MapAsync_Maps_Non_Async_Function_While_Extracting_Task() {
        string hello = "hello";
        Task<Result<string>> taskResult = Task.FromResult(Result<string>.Ok(hello));
        Task<Result<int>> intTaskResult = taskResult.MapAsync(s => s.Length);
        Result<int> intResult = intTaskResult.Result;
        Assert.True(intResult.Succeeded);
        Assert.Equal(hello.Length, intResult.Data);
    }

    [Fact]
    public void MapAsync_Turns_Sync_Result_To_TaskResult() {
        string hello = "hello";
        Result<string> helloResult = Result<string>.Ok(hello);

        async Task<int> SomeAsyncFunction(string s) {
            return s.Length;
        }

        Task<Result<int>> intTaskResult = helloResult.MapAsync(SomeAsyncFunction);
        Result<int> intResult = intTaskResult.Result;
        Assert.True(intResult.Succeeded);
        Assert.Equal(hello.Length, intResult.Data);
    }

    [Fact]
    public void Result_Async_Map_On_Async_Result_Works() {
        string hello = "hello";
        Task<Result<string>> taskResult = Task.FromResult(Result<string>.Ok(hello));

        async Task<Result<int>> SomeAsyncFunction(string s) {
            return Result<int>.Ok(s.Length);
        }

        Task<Result<int>> intTaskResult = taskResult.MapAsync(SomeAsyncFunction);
        Result<int> intResult = intTaskResult.Result;
        Assert.True(intResult.Succeeded);
        Assert.Equal(hello.Length, intResult.Data);
    }

    [Fact]
    public void Result_Map_On_Async_Result_Works() {
        string hello = "hello";
        Task<Result<string>> taskResult = Task.FromResult(Result<string>.Ok(hello));
        Task<Result<int>> intTaskResult = taskResult.MapAsync(s => s.Length);
        Result<int> intResult = intTaskResult.Result;
        Assert.True(intResult.Succeeded);
        Assert.Equal(hello.Length, intResult.Data);
    }
}