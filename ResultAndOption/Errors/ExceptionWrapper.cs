namespace Results;

public record ExceptionWrapper : IError {
    public ExceptionWrapper(Exception exception) {
        Exception = exception;
    }

    public Exception Exception { get; }
    public string Message => Exception.Message;
}