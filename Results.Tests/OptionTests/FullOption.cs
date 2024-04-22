using ResultAndOption.Options.Extensions;

namespace Results.Tests.OptionTests;

public class FullOption {
    private static readonly Option<string> Full = Option<string>.Some("Hello");

    [Fact]
    public void IsSome_Returns_True() {
        Assert.True(Full.IsSome());
    }

    [Fact]
    public void IsNone_Returns_False() {
        Assert.False(Full.IsNone());
    }

    [Fact]
    public void Mapping_With_Func_Returns_Option_Of_New_Value() {
        Option<int> mapped = Full.Map(str => str.Length);
        Assert.True(mapped.IsSome());
        Assert.Equal(Full.Data.Length, mapped.Data);
    }

    [Fact]
    public void Or_Returns_Original_Value() {
        string passed = "new value";
        string or = Full.Or(passed);
        Assert.Equal(Full.Data, or);
    }

    [Fact]
    public void Or_Nullable_Returns_Original_Option() {
        string? nullable = "other";
        string? emptyNullable = null;
        Option<string> or = Full.OrNullable(nullable);
        Option<string> orNullable = Full.OrNullable(emptyNullable);
        Assert.Equal(Full.Data, or.Data);
        Assert.Equal(Full.Data, orNullable.Data);
    }

    [Fact]
    public void OrOption_Returns_Original_Option() {
        //Given
        Option<string> empty = Option<string>.None();
        Option<string> nonEmpty = Option<string>.Some("otherData");
        //When
        Option<string> or = Full.OrOption(nonEmpty);
        Option<string> orEmpty = Full.OrOption(empty);
        //Then
        Assert.Equal(Full.Data, or.Data);
        Assert.Equal(Full.Data, orEmpty.Data);
    }

    [Fact]
    public void Changing_Reference_Of_Variable_To_Null_Does_Not_Affect_Option() {
        //Given
        string? s = "hello";
        Option<string> some = s.ToOption();
        //When
        s = null;
        //Then
        Assert.True(some.IsSome());
        Assert.Equal("hello", some.Data);
    }
}