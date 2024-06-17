namespace ResultAndOption.Results.Commands;

public interface ICommand
{
    public Result Do();
}

public interface ICommand<in T> where T : notnull
{
    public Result Do(T data);
}