namespace Results;

public class ErrorWrapper : Exception {
    public ErrorWrapper(IError error) : base(error.Message) {
    }

    public static Exception Create(IError error) {
        if (error is ExceptionWrapper ex) {
            return ex.Exception;
        }
        else return new ErrorWrapper(error);
    }
}