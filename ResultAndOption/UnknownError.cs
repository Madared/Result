namespace Results;

public class UnknownError : IError
{
    private string _message = "Unknwon Error";
    public string Message => _message;

    public UnknownError()
    {
    }

    public void Log() => Console.WriteLine(_message);
}
