using ResultAndOption.Results;

namespace Results.Context.ActionCallables;

internal sealed class ActionCallableWithInput<TIn> : IActionCallable where TIn : notnull
{
    private readonly Action<TIn> _action;
    private readonly Result<TIn> _result;

    public ActionCallableWithInput(Result<TIn> result, Action<TIn> action)
    {
        _result = result;
        _action = action;
    }

    public void Call()
    {
        if (_result.Failed) return;
        _action(_result.Data);
    }
}