namespace Results;

public class ExceptionWrapper : IError {
    public readonly Exception Exception;
    public ExceptionWrapper(Exception exception) {
        Exception = exception;
    }
    public string Message => Exception.Message;
}