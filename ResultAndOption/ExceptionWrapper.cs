namespace Results;

public record ExceptionWrapper : IError {
    public Exception Exception { get; }

    public ExceptionWrapper(Exception exception) {
        Exception = exception;
    }
    public string Message => Exception.Message;
}