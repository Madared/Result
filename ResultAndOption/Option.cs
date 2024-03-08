namespace Results;

public class Option<T> where T : notnull
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
    public static Option<T> None() => new(default, true);
    public static Option<T> Maybe(T? data) => data is null ? None() : Some(data);

    public Option<TOut> Map<TOut>(Func<T, TOut> function) where TOut : notnull =>
        _data is null || _isNone
            ? Option<TOut>.None()
            : Option<TOut>.Some(function(_data));

    public Option<TOut> Map<TOut>(Func<T, Option<TOut>> function) where TOut : notnull =>
        _data is null || _isNone
            ? Option<TOut>.None()
            : function(_data);

    public Option<T> UseData(Action<T> action)
    {
        if (_data is not null) action(_data);
        return this;
    }

    public bool IsSome() => !_isNone && _data is not null;
    public bool IsNone() => _isNone || _data is null;

    public T Or(T defaultValue) => _data ?? defaultValue;
    public Option<T> OrNullable(T? replacement) => IsNone() ? Maybe(replacement) : this;
    public Option<T> OrOption(Option<T> replacement) => IsNone() ? replacement : this;
}