using Results.CallableGenerators;
using Results.ContextResultExtensions;

namespace Results;

internal class SimpleContextResult : IContextResult {
    private readonly Option<IContextResult> _previousContext;
    private readonly ICommand _command;
    private readonly ICommandGenerator _commandGenerator;
    private readonly Result _result;
    public bool Succeeded => _result.Succeeded;
    public bool Failed => _result.Failed;
    public IError Error => _result.Error;

    public SimpleContextResult(Option<IContextResult> previousContext, ICommand command, ICommandGenerator commandGenerator, Result result) {
        _previousContext = previousContext;
        _command = command;
        _result = result;
        _commandGenerator = commandGenerator;
    }

    public Result StripContext() => _result;

    public IContextResult Do(ICommandGenerator commandGenerator) {
        ICommand command = commandGenerator.Generate();
        return Failed
            ? new SimpleContextResult(this.ToOption<IContextResult>(), command, commandGenerator, Result.Fail(Error))
            : new SimpleContextResult(this.ToOption<IContextResult>(), command, commandGenerator, command.Call());
    }

    public IContextResult<TOut> Map<TOut>(ICallableGenerator<TOut> callableGenerator) where TOut : notnull {
        IContextCallable<TOut> callable = callableGenerator.Generate();
        return Failed 
                ? new ContextResult<TOut>(callable, this.ToOption<IContextResult>(), Result<TOut>.Fail(Error), callableGenerator, new ResultEmitter<TOut>())
                : new ContextResult<TOut>(callable, this.ToOption<IContextResult>(), callable.Call(), callableGenerator, new ResultEmitter<TOut>());
    }

    IContextResult IContextResult.Retry() => Retry();

    public SimpleContextResult Retry() {
        if (Succeeded) return this;
        if (_previousContext.IsNone()) return new SimpleContextResult(Option<IContextResult>.None(), _command, _commandGenerator, _command.Call());
        IContextResult retried = _previousContext.Data.Retry();
        if (retried.Failed) return new SimpleContextResult(_previousContext, _command, _commandGenerator, Result.Fail(retried.Error));
        ICommand command = _commandGenerator.Generate();
        return new SimpleContextResult(_previousContext, command, _commandGenerator, _command.Call());
    }
}