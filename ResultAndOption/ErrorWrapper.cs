namespace Results;

public class ErrorWrapper : Exception {
    public ErrorWrapper(IError error) : base(error.Message) {
        Error = error;
    }

    public IError Error { get; }

    public static Exception Create(IError error) {
        if (error is ExceptionWrapper ex) return ex.Exception;
        return new ErrorWrapper(error);
    }
}