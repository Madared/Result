using Results.CallableGenerators;
using Results.ContextResultExtensions;

namespace Results;

public interface IContextResult : IResult {
    IContextResult Retry();
    Result StripContext();
    void Undo();
    internal IContextResult Do(ICommandGenerator commandGenerator);
    internal IContextResult<TOut> Map<TOut>(ICallableGenerator<TOut> callableGenerator) where TOut : notnull;
}

public interface IContextResult<TOut> : IContextResult, IResult<TOut> where TOut : notnull {
    ResultEmitter<TOut> Emitter { get; }

    IContextResult IContextResult.Retry() {
        return Retry();
    }

    Result IContextResult.StripContext() {
        return StripContext().ToSimpleResult();
    }

    IContextResult IContextResult.Do(ICommandGenerator commandGenerator) {
        return Do(commandGenerator);
    }

    IContextResult<TOut> Retry();
    Result<TOut> StripContext();
    internal IContextResult<TOut> Do(ICommandGenerator commandGenerator);
}