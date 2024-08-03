using ResultAndOption.Errors;
using ResultAndOption.Results;
using ResultAndOption.Results.Commands;
using ResultAndOption.Results.GenericResultExtensions;
using ResultAndOption.Results.GenericResultExtensions.Async;

namespace Results.Tests;

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
        Assert.Throws<ErrorWrapper>(() => stringResult.Data);
    }

    [Fact]
    public void Accessing_Data_In_Failed_Result_Of_Non_Nullable_Value_Type_Throws()
    {
        //Given
        Result<int> intResult = Result<int>.Fail(new UnknownError());
        //Then
        Assert.Throws<ErrorWrapper>(() => intResult.Data);
    }


    [Fact]
    public void Failure_Result_IfFailed_Completes_Action_And_Returns_Self()
    {
        //Given
        Result<string> stringResult = Result<string>.Fail(new UnknownError());
        //When
        string errorMessage = "";
        Result<string> afterActionResult = stringResult
            .OnError(error => errorMessage = error.Message);
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

        Result GetSuccess(string world)
        {
            helloWorld += $" {world}";
            return Result.Ok();
        }

        Result GetFailure()
        {
            return Result.Fail(new UnknownError());
        }

        Result<string> stringResult = Result<string>.Ok("world");
        //When
        Result postGettingSuccess = stringResult.Do(GetSuccess);
        Result postGettingFailure = stringResult.Do(GetFailure);
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
            .Map(data => data.Length);
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

    [Fact]
    public void Aggregation_With_Tuples()
    {
        Result<(string str, int number, string str2)> tupleResult = Result<string>.Ok("hello")
            .Map(str => (str, number: 10))
            .Map(data => (data.str, data.number, str2: "hello"));
        Assert.True(tupleResult.Succeeded);
        Assert.Equal("hello", tupleResult.Data.str);
        Assert.Equal(10, tupleResult.Data.number);
        Assert.Equal("hello", tupleResult.Data.str2);
    }

    [Fact]
    public void ToResult_Works_ToCreate_ResultList()
    {
        //Given
        List<Result<string>> stringResults = new()
        {
            Result<string>.Ok("hello"), Result<string>.Ok("world")
        };
        //When
        ResultList<string> resultList = stringResults.ToResultList();
        //Then
        Assert.False(resultList.HasErrors());
        Assert.Equal(2, resultList.Successes.Count);
    }

    [Fact]
    public void Or_On_Failed_Result_Returns_Passed_In_Value()
    {
        Result<int> failed = Result<int>.Fail(new UnknownError());
        int fromOr = failed.Or(5);
        Assert.Equal(5, fromOr);
    }

    [Fact]
    public void Or_On_Successful_Result_Returns_Internal_Value()
    {
        Result<int> successful = Result<int>.Ok(100);
        int fromOr = successful.Or(5);
        Assert.Equal(100, fromOr);
    }

    [Fact]
    public void MapAsync_Maps_Internal_Result_Extracting_Task()
    {
        Result<string> stringResult = Result<string>.Ok("hello");
        Task<Result<string>> taskResult = Task.FromResult(stringResult);

        Task<Result<int>> intResultTask = taskResult.MapAsync(SomeAsyncFunc);
        Result<int> intResult = intResultTask.Result;
        Assert.True(intResult.Succeeded);
        Assert.Equal(stringResult.Data.Length, intResult.Data);
        return;

        Task<int> SomeAsyncFunc(string f)
        {
            return Task.FromResult(f.Length);
        }
    }

    [Fact]
    public void MapAsync_Maps_Non_Async_Function_While_Extracting_Task()
    {
        string hello = "hello";
        Task<Result<string>> taskResult = Task.FromResult(Result<string>.Ok(hello));
        Task<Result<int>> intTaskResult = taskResult.MapAsync(s => s.Length);
        Result<int> intResult = intTaskResult.Result;
        Assert.True(intResult.Succeeded);
        Assert.Equal(hello.Length, intResult.Data);
    }

    [Fact]
    public void MapAsync_Turns_Sync_Result_To_TaskResult()
    {
        string hello = "hello";
        Result<string> helloResult = Result<string>.Ok(hello);

        Task<Result<int>> intTaskResult = helloResult.MapAsync(SomeAsyncFunction);
        Result<int> intResult = intTaskResult.Result;
        Assert.True(intResult.Succeeded);
        Assert.Equal(hello.Length, intResult.Data);
        return;

        Task<int> SomeAsyncFunction(string s)
        {
            return Task.FromResult(s.Length);
        }
    }

    [Fact]
    public void Result_Async_Map_On_Async_Result_Works()
    {
        string hello = "hello";
        Task<Result<string>> taskResult = Task.FromResult(Result<string>.Ok(hello));

        Task<Result<int>> intTaskResult = taskResult.MapAsync(SomeAsyncFunction);
        Result<int> intResult = intTaskResult.Result;
        Assert.True(intResult.Succeeded);
        Assert.Equal(hello.Length, intResult.Data);
        return;

        Task<Result<int>> SomeAsyncFunction(string s)
        {
            return Task.FromResult(Result<int>.Ok(s.Length));
        }
    }

    [Fact]
    public void Result_Map_On_Async_Result_Works()
    {
        string hello = "hello";
        Task<Result<string>> taskResult = Task.FromResult(Result<string>.Ok(hello));
        Task<Result<int>> intTaskResult = taskResult.MapAsync(s => s.Length);
        Result<int> intResult = intTaskResult.Result;
        Assert.True(intResult.Succeeded);
        Assert.Equal(hello.Length, intResult.Data);
    }

    [Fact]
    public void Result_Map_On_Async_Result_Does_Not_Wrap_Simple_Result()
    {
        string hello = "hello";
        Task<Result<string>> taskResult = Task.FromResult(Result<string>.Ok(hello));
        Task<Result> simpleResult = taskResult.DoAsync(() => Result.Ok());
        Result awaitedResult = simpleResult.Result;
        Assert.True(awaitedResult.Succeeded);
    }

    [Fact]
    public void Result_Map_With_Async_Mapper_Does_Not_Wrap_Simple_Result()
    {
        string hello = "hello";
        Result<string> stringResult = Result<string>.Ok(hello);
        Task<Result> simpleResult = stringResult.DoAsync(_ => Task.FromResult(Result.Ok()));
        Result awaitedResult = simpleResult.Result;
        Assert.True(awaitedResult.Succeeded);
    }

    [Fact]
    public void ErrorWrap_Doesnt_Change_Success_Result()
    {
        Result<string> helloResult = Result<string>.Ok("hello");
        Result<string> wrappedResult =
            helloResult.WrapError<UnknownError>(error => new MultipleErrors(new List<IError>()));
        Assert.True(wrappedResult.Succeeded);
        Assert.Throws<InvalidOperationException>(() => wrappedResult.Error);
    }

    [Fact]
    public void ErrorWrap_Converts_Failed_Result_Error()
    {
        Result<string> helloResult = Result<string>.Fail(new UnknownError());
        Result<string> wrapped =
            helloResult.WrapError<UnknownError>(error => new MultipleErrors(new List<IError>()));
        Assert.True(wrapped.Failed);
        Assert.True(wrapped.Error is MultipleErrors);
    }

    [Fact]
    public void ErrorWrap_Doesnt_Change_Success_On_SimpleResult()
    {
        Result success = Result.Ok();
        Result wrapped = success.WrapError<UnknownError>(error => new ExceptionWrapper(new Exception()));
        Assert.True(wrapped.Succeeded);
        Assert.Throws<InvalidOperationException>(() => wrapped.Error);
    }

    [Fact]
    public void ErrorWrap_Converts_Failed_Simple_Result()
    {
        Result failure = Result.Fail(new UnknownError());
        Result wrapped = failure.WrapError<UnknownError>(error => new ExceptionWrapper(new Exception()));
        Assert.True(wrapped.Failed);
        Assert.True(wrapped.Error is ExceptionWrapper);
    }

    [Fact]
    public void Default_Generic_Result_Constructor_Builds_Failed_Result()
    {
        Result<string> stringResult = new();
        Assert.True(stringResult.Failed);
    }

    [Fact]
    public void Default_SimpleResult_Constructor_Builds_Failed_Result()
    {
        Result result = new();
        Assert.True(result.Failed);
    }

    [Fact]
    public void Default_Constructor()
    {
        Result result = default;
        Result<string> resultString = default;
        Assert.True(result.Failed);
        Assert.True(resultString.Failed);
        Assert.Equal(typeof(UnknownError), result.Error.GetType());
        Assert.Equal(typeof(UnknownError), resultString.Error.GetType());
    }

    [Fact]
    public void Setting_Data_Reference_To_Null_Doesnt_Change_Result_Data()
    {
        string? s = "hello";
        Result<string> sResult = s.ToResult(new UnknownError());
        s = null;
        Assert.True(sResult.Succeeded);
        Assert.Equal("hello", sResult.Data);
    }

    [Fact]
    public void MapWrap_Wraps_Results()
    {
        Result<string> stringResult = Result<string>.Ok("hello");
        Result<Result<string>> mapWrapResult = stringResult.Wrap(str => Result<string>.Ok(str + "boom"));
        Assert.True(mapWrapResult.Succeeded);
        Assert.True(mapWrapResult.Data.Succeeded);
    }
    
    [Fact]
    public void Result_Do_Triggers_On_Success_Result()
    {
        ValueEmitter<bool> emitter = new(false);
        Result<string> stringResult = Result<string>.Ok("hello");
        Result triggered = stringResult.Do(new TriggerCommand<string>(emitter));
        Assert.True(stringResult.Succeeded);
        Assert.True(emitter.Emmit());
    }

    [Fact]
    public void Result_Do_Does_Not_Trigger_On_Failed_Result()
    {
        ValueEmitter<bool> emitter = new(false);
        Result<string> stringResult = Result<string>.Fail(new UnknownError());
        Result triggered = stringResult.Do(new TriggerCommand<string>(emitter));
        Assert.True(stringResult.Failed);
        Assert.False(emitter.Emmit());
    }
    
    [Fact]
    public void Aggregate_Results_Only_Selects_From_Failed_Results()
    {
        Result<string> failedResult = Result<string>.Fail(new UnknownError());
        Result<string> successresult = Result<string>.Ok("hello");
        List<IError> errors = ResultAndOption.Results.GenericResultExtensions.Failing.AggregateErrors(failedResult, successresult).ToList();
        Assert.Single(errors);
    }
}

public sealed class ValueEmitter<T> where T : notnull
{
    private T _value;

    public ValueEmitter(T value)
    {
        _value = value;
    }
    public T Emmit() => _value;
    public void Set(T value) => _value = value;
}

public sealed class TriggerCommand<T> : IResultCommand<T> where T : notnull
{
    private readonly ValueEmitter<bool> _trigger;
    public TriggerCommand(ValueEmitter<bool> trigger)
    {
        _trigger = trigger;
    }

    public Result Do(T data)
    {
        _trigger.Set(true);
        return Result.Ok();
    }
}