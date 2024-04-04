namespace Results;

public interface IContextResultWithData<T> : IContextResult where T : notnull {
    T Data { get; }
    IContextResultWithData<T> Retry();
}