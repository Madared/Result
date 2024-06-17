namespace ResultAndOption.Results.Commands;

public interface IAsyncActionCommand<in T> where T : notnull
{
    public Task Do(T data);
}

public interface IAsyncActionCommand
{
    public Task Do();
}