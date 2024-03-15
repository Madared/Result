namespace Results;

public class Option<T> where T : notnull {
    private readonly T? _data;
    private readonly bool _isNone;

    private Option(T? data, bool isNone) {
        _data = data;
        _isNone = isNone;
    }

    public T Data => _data is null || _isNone
        ? throw new InvalidOperationException()
        : _data;

    public static Option<T> Some(T data) {
        return new Option<T>(data, false);
    }

    public static Option<T> None() {
        return new Option<T>(default, true);
    }

    public static Option<T> Maybe(T? data) {
        return data is null ? None() : Some(data);
    }

    public Option<TOut> Map<TOut>(Func<T, TOut> function) where TOut : notnull {
        return _data is null || _isNone
            ? Option<TOut>.None()
            : Option<TOut>.Some(function(_data));
    }

    public Option<TOut> Map<TOut>(Func<T, Option<TOut>> function) where TOut : notnull {
        return _data is null || _isNone
            ? Option<TOut>.None()
            : function(_data);
    }

    public Option<T> UseData(Action<T> action) {
        if (_data is not null) action(_data);
        return this;
    }

    public bool IsSome() {
        return !_isNone && _data is not null;
    }

    public bool IsNone() {
        return _isNone || _data is null;
    }

    public T Or(T defaultValue) {
        return IsNone() ? defaultValue : Data;
    }

    public Option<T> OrNullable(T? replacement) {
        return IsNone() ? Maybe(replacement) : this;
    }

    public Option<T> OrOption(Option<T> replacement) {
        return IsNone() ? replacement : this;
    }

    public async Task<Option<TOut>> MapAsync<TOut>(Func<T, Task<TOut?>> asyncMapper) where TOut : notnull {
        if (_isNone) {
            return Option<TOut>.None();
        }

        TOut? mapResult = await asyncMapper(Data);
        return mapResult is null ? Option<TOut>.None() : Option<TOut>.Some(mapResult);
    }

    public async Task<Option<TOut>> MapAsync<TOut>(Func<T, Task<Option<TOut>>> asyncMapper) where TOut : notnull {
        if (_isNone) {
            return Option<TOut>.None();
        }

        return await asyncMapper(Data);
    }
}