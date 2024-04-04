namespace Results;

public class ExceptionWrapper : IError {
    public Exception Exception { get; }

    public ExceptionWrapper(Exception exception) {
        Exception = exception;
    }

    public string Message => Exception.Message;

    public void Log(IErrorLogger logger) {
        logger.LogError(Message);
    }
}