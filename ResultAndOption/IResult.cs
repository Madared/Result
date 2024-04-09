namespace Results;

public interface IResult {
    bool Succeeded { get; }
    bool Failed { get; }
    IError Error { get; }
}

public interface IResult<T> : IResult where T : notnull {
    T Data { get; }
}