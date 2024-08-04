using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ResultAndOption.Errors;

public abstract class TraceableError : IError
{
    public string Message { get; }
    public int LineNumber { get; }
    public string FileName { get; }
    public string MethodName { get; }
    public StackTrace StackTrace { get; }

    private TraceableError([CallerFilePath] string fileName = "", [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string methodName = "")
    {
        LineNumber = lineNumber;
        FileName = fileName;
        MethodName = methodName;
        StackTrace = new StackTrace(true);
    }

    public TraceableError(string message) : this()
    {
        Message = message;
    }
}
