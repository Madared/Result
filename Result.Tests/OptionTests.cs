namespace ResultTests;

public class OptionTests {
    [Fact]
    public void Null_Option_IsNone() {
        //Given
        Option<string> stringOption = Option<string>.Maybe(null);
        //When
        bool isNone = stringOption.IsNone();
        bool isSome = stringOption.IsSome();
        //Then
        Assert.True(isNone);
        Assert.False(isSome);
    }

    [Fact]
    public void Non_Null_Option_IsSome() {
        //Given
        Option<string> stringOption = Option<string>.Maybe("hello");
        //When
        bool isSome = stringOption.IsSome();
        bool isNone = stringOption.IsNone();
        //Then
        Assert.True(isSome);
        Assert.False(isNone);
    }

    [Fact]
    public void Null_ValueType_Options_IsNone() {
        //Given
        //Started with string because of notnull constraint in the option type as it cannot be Option<int?>
        Option<int> intOption = Option<string>.None()
            .Map(str => str.Length);
        //When
        bool isNone = intOption.IsNone();
        //Then
        Assert.True(isNone);
    }

    [Fact]
    public void Data_Accessing_In_NoneOption_Throws() {
        //Given
        Option<string> nullOption = Option<string>.None();
        //Then
        Assert.Throws<InvalidOperationException>(() => nullOption.Data);
    }

    [Fact]
    public void Option_Map_On_None_Stays_None() {
        //Given
        Option<string> stringOption = Option<string>.None();
        //When
        Option<string> mappedOption = stringOption.Map(data => data + "hello");
        //Then
        Assert.True(mappedOption.IsNone());
    }

    [Fact]
    public void Option_Map_On_Some_Returns_Proper_Value() {
        //Given
        Option<string> stringOption = Option<string>.Some("hello");
        //When
        Option<string> mappedOption = stringOption.Map(data => data + " world");
        //Then
        Assert.Equal("hello world", mappedOption.Data);
    }

    [Fact]
    public void Mapping_With_Option_Returning_Function_Does_Not_Nest_Options() {
        //Given
        Option<string> stringOption = Option<string>.Some("hello");
        //When
        Option<string> mappedOption = stringOption.Map(data => Option<string>.Some("hello world"));
        //Then
        Assert.Equal("hello world", mappedOption.Data);
    }

    [Fact]
    public void None_Method_Returns_Null_For_Non_Nullable_Types() {
        //Given
        Option<int> intOption = Option<int>.None();
        //When
        bool isNone = intOption.IsNone();
        //Then
        Assert.True(isNone);
        Assert.Throws<InvalidOperationException>(() => intOption.Data);
    }

    [Fact]
    public void Mapping_Non_Nullable_Type_None_Returns_None() {
        //Given
        Option<int> intOption = Option<int>.None();
        //When
        Option<int> mappedOption = intOption.Map(data => data++);
        bool isNone = mappedOption.IsNone();
        //Then
        Assert.True(isNone);
    }

    [Fact]
    public void Or_Gets_Default_Value() {
        //Given
        Option<string> optionalName = Option<string>.None();
        Option<int> optionalInt = Option<int>.None();
        string newName = "newName";
        int newInt = 5;
        //When
        string returned = optionalName.Or(newName);
        int returnedInt = optionalInt.Or(newInt);
        //Then
        Assert.Equal(newName, returned);
        Assert.Equal(newInt, returnedInt);
    }

    [Fact]
    public void Or_Of_Populated_Option_Returns_Internal_Value() {
        //Given
        string initialName = "initialName";
        Option<string> optionalName = Option<string>.Some(initialName);
        string newName = "newName";
        //When
        string returned = optionalName.Or(newName);
        //Then
        Assert.Equal(initialName, returned);
    }

    [Fact]
    public void OrOption_Of_Empty_Option_Gets_Replacement_Value() {
        //Given
        Option<string> optionalString = Option<string>.None();
        Option<string> replacement = Option<string>.Some("hello");
        Option<int> optionalInt = Option<int>.None();
        Option<int> replacementInt = Option<int>.Some(5);
        //When
        Option<string> returned = optionalString.OrOption(replacement);
        Option<int> returnedInt = optionalInt.OrOption(replacementInt);
        //Then
        Assert.Equal(replacement, returned);
        Assert.Equal(replacementInt, returnedInt);
    }

    [Fact]
    public void Or_Option_Of_Populated_Option_Returns_Original_Value() {
        //Given
        Option<string> optionalString = Option<string>.Some("hello");
        Option<int> optionalInt = Option<int>.Some(4);
        Option<string> replacementString = Option<string>.Some("world");
        Option<int> replacementInt = Option<int>.Some(20);
        //When
        Option<string> returnedString = optionalString.OrOption(replacementString);
        Option<int> returnedInt = optionalInt.OrOption(replacementInt);
        //Then
        Assert.Equal(optionalString, returnedString);
        Assert.Equal(optionalInt, returnedInt);
    }

    [Fact]
    public void OrNullable_Of_Empty_Option_Gets_Replacement_Value() {
        //Given
        Option<string> optionalString = Option<string>.None();
        string replacementString = "hello";
        //When
        Option<string> returnedString = optionalString.OrNullable(replacementString);
        //Then
        Assert.Equal(replacementString, returnedString.Data);
    }

    [Fact]
    public void OrNullable_With_Null_Replacement_Returns_Empty_Option() {
        //Given
        Option<string> optionalString = Option<string>.None();
        string? replacement = null;
        //When
        Option<string> returnedString = optionalString.OrNullable(replacement);
        //Then
        Assert.True(returnedString.IsNone());
    }

    [Fact]
    public void ToOption_Turns_Null_Into_Empty_Option() {
        string? nullString = null;
        Option<string> optionalString = nullString.ToOption();
        Assert.True(optionalString.IsNone());
    }

    [Fact]
    public void ToOption_Turns_Existing_Value_Into_Full_Option() {
        string? existingString = "hello";
        Option<string> optionalString = existingString.ToOption();
        Assert.True(optionalString.IsSome());
    }

    [Fact]
    public void Async_Option_Map_Returns_Correct_Value() {
        string hello = "hello";
        Task<Option<string>> taskOption = Task.FromResult(Option<string>.Some(hello));
        Task<Option<int>> intTaskOption = taskOption.MapAsync(s => s.Length);
        Option<int> intOption = intTaskOption.Result;
        Assert.True(intOption.IsSome());
        Assert.Equal(hello.Length, intOption.Data);
    }

    [Fact]
    public void Async_Option_Map_With_Async_Mapper_Returns_Correct_Value() {
        string hello = "hello";
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
        string hello = "hello";
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