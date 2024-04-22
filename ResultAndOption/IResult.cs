namespace Results;

public interface IMapper<out TOut> where TOut : notnull {
    TOut Map();
}

public interface IMapper<in TIn, out TOut> where TIn : notnull where TOut : notnull {
    TOut Map(TIn data);
}

public interface IMappable {
    IMappable<TOut> Map<TOut>(IMapper<TOut> mapper) where TOut : notnull;
}

public interface IMappable<out TIn> where TIn : notnull {
    IMappable<TOut> Map<TOut>(IMapper<TIn, TOut> mapper) where TOut : notnull;
}

public interface IResult {
    bool Succeeded { get; }
    bool Failed { get; }
    IError Error { get; }
}

public interface IResult<out T> : IResult {
    T Data { get; }
}