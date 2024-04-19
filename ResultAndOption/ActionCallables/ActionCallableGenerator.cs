namespace Results.ActionCallables;

internal sealed class ActionCallableGenerator : IActionCallableGenerator {
    private readonly Action _action;

    public ActionCallableGenerator(Action action) {
        _action = action;
    }

    public IActionCallable Generate() {
        return new ActionCallable(_action);
    }
}