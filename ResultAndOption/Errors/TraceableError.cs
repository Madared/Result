using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ResultAndOption.Errors;

public abstract class TraceableError : IError
{
    public string Message { get; }
    /// <summary>
    /// The Line number where the Error was created
    /// </summary>
    public int LineNumber { get; }
    /// <summary>
    /// The File name where the error was created
    /// </summary>
    public string FileName { get; }
    /// <summary>
    /// The method name where the error was created
    /// </summary>
    public string MethodName { get; }
    /// <summary>
    /// The stack trace at the moment the error was created
    /// </summary>
    public StackTrace StackTrace { get; }

    /// <summary>
    /// Private constructor which automatically populates the information fields of the traceable error
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="lineNumber"></param>
    /// <param name="methodName"></param>
    private TraceableError([CallerFilePath] string fileName = "", [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string methodName = "")
    {
        LineNumber = lineNumber;
        FileName = fileName;
        MethodName = methodName;
        StackTrace = new StackTrace(true);
    }

    /// <summary>
    /// Traceable error constructor
    /// </summary>
    /// <param name="message"></param>
    public TraceableError(string message) : this()
    {
        Message = message;
    }
}
