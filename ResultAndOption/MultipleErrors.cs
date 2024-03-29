namespace Results;

public class MultipleErrors : IError {
    private readonly IEnumerable<IError> _errors;

    public MultipleErrors(IEnumerable<IError> errors) {
        _errors = errors;
    }

    public string Message => string.Format("{0} : \n {1}",
        "The following Errors have occurred",
        _errors
            .ListMap(error => error.Message)
            .PipeNonNull(errorMessages => string.Join(",\n", errorMessages)));

    public void Log(IErrorLogger logger) {
        logger.LogError(Message);
    }
}