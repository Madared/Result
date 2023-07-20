namespace Results;

public class Option<T>
{
    private readonly T? _data;
    private readonly bool _isNone;

    public T Data => _data is null || _isNone
        ? throw new InvalidOperationException()
        : _data;

    private Option(T? data, bool isNone)
    {
        _data = data;
        _isNone = isNone;
    }

    public static Option<T> Some(T data) => new(data, false);
    public static Option<T> None() => new(default(T?), true);
    public static Option<T> Maybe(T? data) =>
        data is null
            ? Option<T>.None()
            : Option<T>.Some(data);

    public Option<TOut> Map<TOut>(Func<T, TOut> function) =>
        _data is null || _isNone
            ? Option<TOut>.None()
            : Option<TOut>.Some(function(_data));

    public Option<TOut> Map<TOut>(Func<T, Option<TOut>> function) =>
        _data is null || _isNone
            ? Option<TOut>.None()
            : function(_data);

    public bool IsSome() => !_isNone && _data is not null;
    public bool IsNone() => _isNone || _data is null;
}
