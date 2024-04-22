using ResultAndOption.Errors;

namespace Results.Context.ContextResults;

public record ContextHasBeenUndone : IError {
    public string Message => "Context has been undone";
}