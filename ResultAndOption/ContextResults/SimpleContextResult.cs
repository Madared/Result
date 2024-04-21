using Results.CallableGenerators;
using Results.ContextResultExtensions;

namespace Results;

internal class SimpleContextResult : IContextResult {
    private readonly ICommand _command;
    private readonly ICommandGenerator _commandGenerator;
    private readonly Option<IContextResult> _previousContext;
    private readonly Result _result;
    private bool _undone;
    private Result ActiveResult => _undone ? Result.Fail(new UnknownError()) : _result;

    public SimpleContextResult(Option<IContextResult> previousContext, ICommand command, ICommandGenerator commandGenerator, Result result) {
        _previousContext = previousContext;
        _command = command;
        _result = result;
        _commandGenerator = commandGenerator;
    }

    public bool Succeeded => ActiveResult.Succeeded;
    public bool Failed => ActiveResult.Failed;
    public IError Error => ActiveResult.Error;

    public Result StripContext() {
        return ActiveResult;
    }

    public void Undo() {
        if (Succeeded) _command.Undo();
        if (_previousContext.IsSome()) _previousContext.Data.Undo();
        _undone = true;
    }

    public IContextResult Do(ICommandGenerator commandGenerator) {
        var command = commandGenerator.Generate();
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

    public IContextResult Retry() {
        if (Succeeded) return this;
        if (_previousContext.IsNone()) return new SimpleContextResult(Option<IContextResult>.None(), _command, _commandGenerator, _command.Call());
        var retried = _previousContext.Data.Retry();
        var command = _commandGenerator.Generate();
        return retried.Failed
            ? new SimpleContextResult(_previousContext, command, _commandGenerator, Result.Fail(retried.Error))
            : new SimpleContextResult(_previousContext, command, _commandGenerator, _command.Call());
    }
}