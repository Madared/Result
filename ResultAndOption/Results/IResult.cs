using ResultAndOption.Errors;

namespace ResultAndOption.Results;
public interface IResult {
    bool Succeeded { get; }
    bool Failed { get; }
    IError Error { get; }
}

public interface IResult<out T> : IResult {
    T Data { get; }
}