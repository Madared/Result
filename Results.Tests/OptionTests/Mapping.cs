using ResultAndOption.Options.Extensions;

namespace Results.Tests.OptionTests;

public class Mapping {
    [Fact]
    public void Mapping_Some_To_Another_Option_Does_Not_Nest() {
        Option<string> some = Option<string>.Some("hello");
        Option<int> mapped = some.Map(str => Option<int>.Some(str.Length));
        Assert.True(mapped.IsSome());
    }

    [Fact]
    public void Mapping_None_To_Another_Option_Does_Not_Nest() {
        Option<string> none = Option<string>.None();
        Option<int> mapped = none.Map(str => Option<int>.Some(str.Length));
        Assert.True(mapped.IsNone());
    }
}