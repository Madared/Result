using ResultAndOption.Options.Extensions.Async;

namespace Results.Tests.OptionTests;

public class AsyncOptionTests {
    [Fact]
    public void Async_Option_Map_Returns_Correct_Value() {
        const string hello = "hello";
        Task<Option<string>> taskOption = Task.FromResult(Option<string>.Some(hello));
        Task<Option<int>> intTaskOption = taskOption.MapAsync(s => s.Length);
        Option<int> intOption = intTaskOption.Result;
        Assert.True(intOption.IsSome());
        Assert.Equal(hello.Length, intOption.Data);
    }

    [Fact]
    public void Async_Option_Map_With_Async_Mapper_Returns_Correct_Value() {
        const string hello = "hello";
        Task<Option<string>> taskOption = Task.FromResult(Option<string>.Some(hello));

        async Task<int> SomeAsyncFunction(string s) {
            return s.Length;
        }

        Task<Option<int>> intTaskOption = taskOption.MapAsync(SomeAsyncFunction);
        Option<int> intOption = intTaskOption.Result;
        Assert.True(intOption.IsSome());
        Assert.Equal(hello.Length, intOption.Data);
    }

    [Fact]
    public void Async_Option_Map_Unwraps_Nested_Option_Result() {
        const string hello = "hello";
        Task<Option<string>> taskOption = Task.FromResult(Option<string>.Some(hello));
        Task<Option<int>> intTaskOption = taskOption.MapAsync(s => Option<int>.Some(s.Length));
        Option<int> intOption = intTaskOption.Result;
        Assert.True(intOption.IsSome());
        Assert.Equal(hello.Length, intOption.Data);
    }

    [Fact]
    public void Async_Option_Map_Unwraps_Nested_Async_Option_Result() {
        string hello = "hello";
        Task<Option<string>> taskOption = Task.FromResult(Option<string>.Some(hello));

        async Task<Option<int>> SomeAsyncFunction(string s) {
            return Option<int>.Some(s.Length);
        }

        Task<Option<int>> intTaskOption = taskOption.MapAsync(SomeAsyncFunction);
        Option<int> intOption = intTaskOption.Result;
        Assert.True(intOption.IsSome());
        Assert.Equal(hello.Length, intOption.Data);
    }
}