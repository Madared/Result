namespace Results;

public class ExceptionWrapper : IError {
    private readonly Exception _exception;

    public ExceptionWrapper(Exception exception) {
        _exception = exception;
    }

    public string Message => _exception.Message;

    public void Log(IErrorLogger logger) {
        logger.LogError(Message);
    }
}