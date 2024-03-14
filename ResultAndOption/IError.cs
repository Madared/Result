namespace Results;

public interface IError {
    public string Message { get; }
    public void Log(IErrorLogger logger);
}