namespace Results;

public class ErrorWrapperException : Exception {
    public ErrorWrapperException(IError error) : base(error.Message) {
        Error = error;
    }

    public IError Error { get; }
}