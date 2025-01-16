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

    /// <summary>
    /// Performs command if option is populated
    /// </summary>
    /// <param name="command">Command to perform</param>
    /// <returns>The same option</returns>
    public Option<T> Do(ICommand command)
    {
        if (IsSome())
        {
            command.Do();
        }

        return this;
    }

    /// <summary>
    /// Performs asynchronous command if option is poulated
    /// </summary>
    /// <param name="command"></param>
    /// <returns>The same option</returns>
    public async Task<Option<T>> DoAsync(IAsyncCommand command)
    {
        if (IsSome())
        {
            await command.Do();
        }

        return this;
    }

    /// <summary>
    /// Performs a command with the option value if it exists
    /// </summary>
    /// <param name="command"></param>
    /// <returns>The same option</returns>
    public Option<T> Do(ICommand<T> command)
    {
        if (IsSome())
        {
            command.Do(Data);
        }

        return this;
    }

    /// <summary>
    /// Performs an asynchronous command with the option value if it exists
    /// </summary>
    /// <param name="command"></param>
    /// <returns>The same option</returns>
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

    /// <summary>
    /// Implicitly converts non null value into a full option
    /// </summary>
    /// <param name="data">Non null value to convert</param>
    /// <returns>Full option</returns>
    public static implicit operator Option<T>(T data) => Some(data);
}