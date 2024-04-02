namespace Results;

public class ExceptionWrapper : IError {
    public readonly Exception Exception;
    public ExceptionWrapper(Exception exception) {
        this.Exception = exception;
    }

    public string Message => Exception.Message;

    public void Log(IErrorLogger logger) {
        logger.LogError(Message);
    }
}