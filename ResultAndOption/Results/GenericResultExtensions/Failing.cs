using ResultAndOption.Errors;

namespace ResultAndOption.Results.GenericResultExtensions;

public static class Failing
{
    public static IEnumerable<IError> AggregateErrors(params IResult[] results) => results
        .Where(r => r.Failed)
        .Select(failed => failed.Error);
}