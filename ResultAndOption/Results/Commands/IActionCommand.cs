namespace ResultAndOption.Results.Commands;

public interface IActionCommand
{
    public void Do();
}

public interface IActionCommand<in T> where T : notnull
{
    public void Do(T data);
}