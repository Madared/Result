namespace ResultTests;

public class OptionTests
{
    [Fact]
    public void Null_Option_IsNone()
    {
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
    public void Non_Null_Option_IsSome()
    {
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
    public void Null_ValueType_Options_IsNone()
    {
        //Given
        int? nullInt = null;
        Option<int?> intOption = Option<int?>.Maybe(nullInt);
        //When
        bool isNone = intOption.IsNone();
        //Then
        Assert.True(isNone);
    }

    [Fact]
    public void Data_Accessing_In_NoneOption_Throws()
    {
        //Given
        Option<string> nullOption = Option<string>.None();
        //Then
        Assert.Throws<InvalidOperationException>(() => nullOption.Data);
    }

    [Fact]
    public void Option_Map_On_None_Stays_None()
    {
        //Given
        Option<string> stringOption = Option<string>.None();
        //When
        Option<string> mappedOption = stringOption.Map(data => data + "hello");
        //Then
        Assert.True(mappedOption.IsNone());
    }

    [Fact]
    public void Option_Map_On_Some_Returns_Proper_Value()
    {
        //Given
        Option<string> stringOption = Option<string>.Some("hello");
        //When
        Option<string> mappedOption = stringOption.Map(data => data + " world");
        //Then
        Assert.Equal("hello world", mappedOption.Data);
    }

    [Fact]
    public void Mapping_With_Option_Returning_Function_Does_Not_Nest_Options()
    {
        //Given
        Option<string> stringOption = Option<string>.Some("hello");
        //When
        Option<string> mappedOption = stringOption.Map(data => Option<string>.Some("hello world"));
        //Then
        Assert.Equal("hello world", mappedOption.Data);
    }

    [Fact]
    public void None_Method_Returns_Null_For_Non_Nullable_Types()
    {
        //Given
        Option<int> intOption = Option<int>.None();
        //When
        bool isNone = intOption.IsNone();
        //Then
        Assert.True(isNone);
        Assert.Throws<InvalidOperationException>(() => intOption.Data);
    }

    [Fact]
    public void Mapping_Non_Nullable_Type_None_Returns_None()
    {
        //Given
        Option<int> intOption = Option<int>.None();
        //When
        Option<int> mappedOption = intOption.Map(data => data++);
        bool isNone = mappedOption.IsNone();
        //Then
        Assert.True(isNone);
    }
}
