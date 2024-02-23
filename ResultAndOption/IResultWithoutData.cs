namespace Results;

public interface IResultWithoutData
{
    bool Succeeded { get; }
    bool Failed { get; }
    IError Error { get; }
}