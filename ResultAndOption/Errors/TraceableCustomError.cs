using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ResultAndOption.Errors;

/// <summary>
/// A traceable custom error with functionality similar to exceptions
/// </summary>
public abstract record TraceableCustomError : CustomError
{
    /// <inheritdoc cref="CustomError.Message"/>
    public override string Message { get; }
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
    private TraceableCustomError([CallerFilePath] string fileName = "", [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string methodName = "")
    {
        Message = "";
        LineNumber = lineNumber;
        FileName = fileName;
        MethodName = methodName;
        StackTrace = new StackTrace(true);
    }

    /// <summary>
    /// Traceable error constructor
    /// </summary>
    /// <param name="message"></param>
    public TraceableCustomError(string message) : this()
    {
        Message = message;
    }
}
