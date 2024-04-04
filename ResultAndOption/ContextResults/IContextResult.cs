namespace Results;

public interface IContextResult {
    bool Succeeded { get; }
    bool Failed { get; }
    IError Error { get; }
}