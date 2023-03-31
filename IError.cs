namespace Result;

///
///<summary>
///This interface is meant to be extended by the consumer of the code.
///</summary>
public interface IError
{
    public string Message { get; }
    public void Log();
}

