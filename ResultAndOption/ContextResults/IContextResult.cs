using ResultAndOption.CallableGenerators;
using ResultAndOption.ContextCommands;
using ResultAndOption.ContextResultExtensions;
using ResultAndOption.Results;
using ResultAndOption.Results.GenericResultExtensions;

namespace ResultAndOption;

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

    new IContextResult<TOut> Retry();
    new Result<TOut> StripContext();
    internal new IContextResult<TOut> Do(ICommandGenerator commandGenerator);
}