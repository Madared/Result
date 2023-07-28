using Microsoft.Extensions.Logging;

namespace Results;

public class UnknownError : IError
{
    private const string _message = "Unknown Error";
    public string Message => _message;

    public UnknownError()
    {
    }
    public void Log(ILogger logger) => logger.LogError(_message);
}