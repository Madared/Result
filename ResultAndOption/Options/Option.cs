using ResultAndOption.Results.Commands;

namespace ResultAndOption.Options;

/// <summary>
/// Representation for an optional value
/// </summary>
public readonly struct Option<T> where T : notnull
{
    private readonly T? _data;
    private readonly bool _isNone;

    private Option(T? data, bool isNone)
    {
        _data = data;
        _isNone = isNone;
    }

    /// <summary>
    /// Gets the data
    /// </summary>
    /// <exception cref="InvalidOperationException">Throws if the data is not present</exception>
    public T Data => _data is null || _isNone
        ? throw new InvalidOperationException("Option is empty!")
        : _data;

    /// <summary>
    /// Checks if the option is populated
    /// </summary>
    /// <returns></returns>
    public bool IsSome() => !_isNone && _data is not null;

    /// <summary>
    /// Checks if the option is empty
    /// </summary>
    /// <returns></returns>
    public bool IsNone() => _isNone || _data is null;

    public Option<T> Do(ICommand command)
    {
        if (IsSome())
        {
            command.Do();
        }

        return this;
    }

    public async Task<Option<T>> DoAsync(IAsyncCommand command)
    {
        if (IsSome())
        {
            await command.Do();
        }

        return this;
    }

    public Option<T> Do(ICommand<T> command)
    {
        if (IsSome())
        {
            command.Do(Data);
        }

        return this;
    }

    public async Task<Option<T>> DoAsync(IAsyncCommand<T> command)
    {
        if (IsSome())
        {
            await command.Do(Data);
        }

        return this;
    }

    /// <summary>
    /// Creates a populated Option
    /// </summary>
    /// <param name="data">The data to populate the option with</param>
    /// <returns></returns>
    public static Option<T> Some(T data) => new(data, false);

    /// <summary>
    /// Creates an empty option
    /// </summary>
    /// <returns></returns>
    public static Option<T> None() => new(default, true);

    /// <summary>
    /// Creates an empty option if the data is null otherwise populates it with data
    /// </summary>
    /// <param name="data">Nullable data to insert into the option</param>
    /// <returns></returns>
    public static Option<T> Maybe(T? data) => data is null ? None() : Some(data);
}