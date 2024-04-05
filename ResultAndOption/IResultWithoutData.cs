namespace Results;

public interface IResultWithoutData {
    bool Succeeded { get; }
    bool Failed { get; }
    IError Error { get; }
}

public interface IResultWithData<T> : IResultWithoutData where T : notnull {
    T Data { get; }
}