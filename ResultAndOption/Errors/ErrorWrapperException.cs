namespace Results;

public class ErrorWrapperException : Exception {
    public IError Error { get; }
    public ErrorWrapperException(IError error) : base(error.Message) {
        Error = error;
    }
}