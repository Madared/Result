namespace Results;

public class UnknownError : IError {
    private const string _message = "Unknown Error";

    public string Message => _message;
}