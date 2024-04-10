namespace ResultTests.OptionTests;

public class EmptyOption {
    private static readonly Option<string> Empty;

    [Fact]
    public void IsNone_Returns_True() {
        Assert.True(Empty.IsNone());
    }

    [Fact]
    public void IsSome_Returns_False() {
        Assert.False(Empty.IsSome());
    }

    [Fact]
    public void Mapping_Gives_Empty_Option() {
        Option<int> mapped = Empty.Map(str => str.Length);
        Assert.True(mapped.IsNone());
    }

    [Fact]
    public void UseData_Doesnt_Run_Action() {
        string? nullable = null;
        Empty.UseData(str => nullable = str);
        Assert.True(nullable is null);
    }

    [Fact]
    public void Or_Returns_Passed_In_Value() {
        string passed = "Hello";
        string or = Empty.Or(passed);
        Assert.Equal(passed, or);
    }

    [Fact]
    public void OrNullable_Returns_Option_With_PassedValue() {
        string updated = "hello";
        Option<string> or = Empty.OrNullable(updated);
        Assert.True(or.IsSome());
        Assert.Equal(updated, or.Data);
    }

    [Fact]
    public void OrNullable_With_Null_Returns_Another_Empty_Option() {
        Option<string> or = Empty.OrNullable(null);
        Assert.True(or.IsNone());
    }

    [Fact]
    public void OrOption_Returns_Passed_Option() {
        Option<string> updated = Option<string>.Some("updated");
        Option<string> or = Empty.OrOption(updated);
        Assert.True(or.IsSome());
        Assert.Equal(updated.Data, or.Data);
    }

    [Fact]
    public void OrOption_With_Empty_Option_Returns_Empty_Option() {
        Option<string> otherEmpty = Option<string>.None();
        Option<string> or = Empty.OrOption(otherEmpty);
        Assert.True(or.IsNone());
    }

    [Fact]
    public void Accessing_Data_Throws() {
        Assert.Throws<InvalidOperationException>(() => Empty.Data);
    }
}