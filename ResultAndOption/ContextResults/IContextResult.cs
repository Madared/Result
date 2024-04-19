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
    IContextResult IContextResult.Retry() => Retry();
    IContextResult<TOut> Retry();
    Result IContextResult.StripContext() => StripContext().ToSimpleResult();
    Result<TOut> StripContext();
    internal IContextResult<TOut> Do(ICommandGenerator commandGenerator);
    IContextResult IContextResult.Do(ICommandGenerator commandGenerator) => Do(commandGenerator);
}