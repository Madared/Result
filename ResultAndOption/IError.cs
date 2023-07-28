namespace Results;
using Microsoft.Extensions.Logging;

public interface IError
{
    public string Message { get; }
    public void Log(ILogger logger);
}
