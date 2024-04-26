using ResultAndOption.Options;
using ResultAndOption.Options.Extensions;

namespace Results.Tests.OptionTests;

public class OptionCreation
{
    [Fact]
    public void None_Creates_Empty_Option()
    {
        Option<string> none = Option<string>.None();
        Assert.True(none.IsNone());
    }

    [Fact]
    public void Some_Creates_Populated_Option()
    {
        Option<string> some = Option<string>.Some("hello");
        Assert.True(some.IsSome());
    }

    [Fact]
    public void Maybe_Of_Null_Creates_Empty_Option()
    {
        Option<string> maybe = Option<string>.Maybe(null);
        Assert.True(maybe.IsNone());
    }

    [Fact]
    public void Maybe_Of_Not_Null_Creates_Populated_Option()
    {
        Option<string> maybe = Option<string>.Maybe("hello");
        Assert.True(maybe.IsSome());
    }

    [Fact]
    public void ToOption_On_Null_Creates_Empty_Option()
    {
        string? nullable = null;
        Option<string> toOption = nullable.ToOption();
        Assert.True(toOption.IsNone());
    }

    [Fact]
    public void ToOption_On_Not_Null_Creates_Populated_Option()
    {
        string some = "hello";
        Option<string> toOption = some.ToOption();
        Assert.True(toOption.IsSome());
    }

    [Fact]
    public void Value_Type_Option_Created_From_None_Is_Empty()
    {
        Option<int> none = Option<int>.None();
        Assert.True(none.IsNone());
    }

    [Fact]
    public void Default_Option_Is_Empty()
    {
        Option<string> empty = default;
        Assert.True(empty.IsNone());
    }
}