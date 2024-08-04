namespace ResultAndOption.Results.Getters;

public interface IGetter<out T> where T : notnull
{
    T Get();
}