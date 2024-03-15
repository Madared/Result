namespace Results;

/// <summary>
/// Representation for an optional value
/// </summary>
public class Option<T> where T : notnull {
    private readonly T? _data;
    private readonly bool _isNone;

    private Option(T? data, bool isNone) {
        _data = data;
        _isNone = isNone;
    }

    /// <summary>
    /// Gets the data
    /// </summary>
    /// <exception cref="InvalidOperationException">Throws if the data is not present</exception>
    public T Data => _data is null || _isNone
        ? throw new InvalidOperationException()
        : _data;

    /// <summary>
    /// Creates a populated Option
    /// </summary>
    /// <param name="data">The data to populate the option with</param>
    /// <returns></returns>
    public static Option<T> Some(T data) {
        return new Option<T>(data, false);
    }

    /// <summary>
    /// Creates an empty option
    /// </summary>
    /// <returns></returns>
    public static Option<T> None() {
        return new Option<T>(default, true);
    }

    /// <summary>
    /// Creates an emtpy option if the data is null otherwise populates it with data
    /// </summary>
    /// <param name="data">Nullable data to insert into the option</param>
    /// <returns></returns>
    public static Option<T> Maybe(T? data) {
        return data is null ? None() : Some(data);
    }

    /// <summary>
    /// Maps the option into the result of the function wrapped in an option, if the options is empty
    /// it foregoes calling the map function and just returns an empty option
    /// </summary>
    /// <param name="function">mapping function</param>
    /// <returns></returns>
    public Option<TOut> Map<TOut>(Func<T, TOut> function) where TOut : notnull {
        return _data is null || _isNone
            ? Option<TOut>.None()
            : Option<TOut>.Some(function(_data));
    }

    /// <summary>
    /// Maps the option into the new Option type returned by the function, foregoes calling the mapper if the option is
    /// empty and just returns a new emtpy option of the new type
    /// </summary>
    /// <param name="function">mapping function</param>
    /// <returns></returns>
    public Option<TOut> Map<TOut>(Func<T, Option<TOut>> function) where TOut : notnull {
        return _data is null || _isNone
            ? Option<TOut>.None()
            : function(_data);
    }

    /// <summary>
    /// Performs the action in case the option is not empty, and returns this option
    /// </summary>
    /// <param name="action">Action to perform</param>
    /// <returns></returns>
    public Option<T> UseData(Action<T> action) {
        if (_data is not null) action(_data);
        return this;
    }

    /// <summary>
    /// Checks if the option is populated
    /// </summary>
    /// <returns></returns>
    public bool IsSome() {
        return !_isNone && _data is not null;
    }

    /// <summary>
    /// Checks if the option is empty
    /// </summary>
    /// <returns></returns>
    public bool IsNone() {
        return _isNone || _data is null;
    }

    /// <summary>
    /// Returns the value inside the option, in case it is empty returns the non-null value passed in
    /// </summary>
    /// <param name="defaultValue">Value to return in case option is empty</param>
    /// <returns></returns>
    public T Or(T defaultValue) {
        return IsNone() ? defaultValue : Data;
    }

    public Option<T> OrNullable(T? replacement) {
        return IsNone() ? Maybe(replacement) : this;
    }

    /// <summary>
    /// Returns this option if it isnt empty, otherwise returns the passed in option
    /// </summary>
    /// <param name="replacement">replacement option</param>
    /// <returns></returns>
    public Option<T> OrOption(Option<T> replacement) {
        return IsNone() ? replacement : this;
    }

    /// <summary>
    /// Maps the Option by implicitly awaiting the asynchronous mapping function, foregoes calling the function if
    /// the option is empty
    /// </summary>
    /// <param name="asyncMapper">asynchronous mapping function</param>
    /// <returns></returns>
    public async Task<Option<TOut>> MapAsync<TOut>(Func<T, Task<TOut?>> asyncMapper) where TOut : notnull {
        if (_isNone) {
            return Option<TOut>.None();
        }

        TOut? mapResult = await asyncMapper(Data);
        return mapResult is null ? Option<TOut>.None() : Option<TOut>.Some(mapResult);
    }

    /// <summary>
    /// Maps the Option by implicitly awaiting the asynchronous mapping function, foregoes calling the function if
    /// the option is empty
    /// </summary>
    /// <param name="asyncMapper">asynchronous mapping function</param>
    /// <returns></returns>
    public async Task<Option<TOut>> MapAsync<TOut>(Func<T, Task<Option<TOut>>> asyncMapper) where TOut : notnull {
        if (_isNone) {
            return Option<TOut>.None();
        }

        return await asyncMapper(Data);
    }
}