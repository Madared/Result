using ResultAndOption;
using ResultAndOption.Results;
using ResultAndOption.Results.GenericResultExtensions;
using Results.Context.CallableGenerators;
using Results.Context.ContextCommands;

namespace Results.Context.ContextResults;

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

    Result IContextResult.StripContext() => StripContext().ToSimpleResult();

    IContextResult IContextResult.Do(ICommandGenerator commandGenerator) => Do(commandGenerator);

    new IContextResult<TOut> Retry();
    new Result<TOut> StripContext();
    internal new IContextResult<TOut> Do(ICommandGenerator commandGenerator);
}