namespace Results;

public static class ErrorToExceptionMapper {
    public static Exception Map(IError? error) => error switch {
        null => new InvalidOperationException(),
        Exception e => e,
        ExceptionWrapper e => e.Exception,
        _ => new ErrorWrapperException(error)
    };
}