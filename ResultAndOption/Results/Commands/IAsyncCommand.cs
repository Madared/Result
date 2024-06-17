namespace ResultAndOption.Results.Commands;

public interface IAsyncCommand<in T> where T : notnull
{
    public Task<Result> Do(T data);
}

public interface IAsyncCommand
{
    public Task<Result> Do();
}