using System.CodeDom.Compiler;

namespace Results;

internal class IntermediateContextResult<TOut> : IContextResult<TOut> where TOut : notnull {
    private readonly Result<TOut> _result;
    private readonly Func<Result<TOut>> _callable;
    private readonly IContextResult _previousContext;

    public IntermediateContextResult(Result<TOut> result, Func<Result<TOut>> callable, IContextResult previousContext) {
        _result = result;
        _callable = callable;
        _previousContext = previousContext;
    }

    public bool Succeeded => _result.Succeeded;
    public bool Failed => _result.Failed;
    public IError Error => _result.Error;
    public TOut Data => _result.Data;

    IContextResult<TOut> IContextResult<TOut>.Retry() => Retry();

    public IntermediateContextResult<TOut> Retry() {
        if (Succeeded) return this;
        IContextResult newContext = _previousContext.Retry();
        return newContext.Failed
            ? new IntermediateContextResult<TOut>(Result<TOut>.Fail(newContext.Error), _callable, newContext)
            : new IntermediateContextResult<TOut>(_callable(), _callable, newContext);
    }

    public Result<TOut> StripContext() => _result;

    public IContextResult<TNext> Map<TNext>(Func<TOut, Result<TNext>> mapper) where TNext : notnull => Failed
        ? new ContextResult<TOut, TNext>(mapper, this, Result<TNext>.Fail(Error))
        : new ContextResult<TOut, TNext>(mapper, this, mapper(Data));

    public IContextResult<TNext> Map<TNext>(Func<TOut, TNext> mapper) where TNext : notnull {
        Func<TOut, Result<TNext>> generated = (TOut input) => {
            TNext data = mapper(input);
            return Result<TNext>.Ok(data);
        };
        return Failed
            ? new ContextResult<TOut, TNext>(generated, this, Result<TNext>.Fail(Error))
            : new ContextResult<TOut, TNext>(generated, this, generated(Data));
    }

    public IContextResult Map(Func<TOut, Result> mapper) => Failed
        ? new IntermediateContextResultSimple<TOut>(mapper, this, Result.Fail(Error))
        : new IntermediateContextResultSimple<TOut>(mapper, this, mapper(Data));

    public IContextResult Map(Action<TOut> mapper) {
        Func<TOut, Result> generated = (data) => {
            mapper(data);
            return Result.Ok();
        };
        return Failed
            ? new IntermediateContextResultSimple<TOut>(generated, this, Result.Fail(Error))
            : new IntermediateContextResultSimple<TOut>(generated, this, generated(Data));
    }

    public IContextResult Map(Action action) {
        Func<Result> generated = () => {
            action();
            return Result.Ok();
        };
        return Failed
            ? new SimpleContextResult(this, generated, Result.Fail(Error))
            : new SimpleContextResult(this, generated, generated());
    }

    public IContextResult Map(Func<Result> mapper) => Failed
        ? new SimpleContextResult(this, mapper, Result.Fail(Error))
        : new SimpleContextResult(this, mapper, mapper());

    public IContextResult<TNext> Map<TNext>(Func<Result<TNext>> mapper) where TNext : notnull => Failed
        ? new IntermediateContextResult<TNext>(Result<TNext>.Fail(Error), mapper, this)
        : new IntermediateContextResult<TNext>(mapper(), mapper, this);

    public IContextResult<TNext> Map<TNext>(Func<TNext> mapper) where TNext : notnull {
        Func<Result<TNext>> generated = () => {
            TNext data = mapper();
            return Result<TNext>.Ok(data);
        };
        return Failed
            ? new IntermediateContextResult<TNext>(Result<TNext>.Fail(Error), generated, this)
            : new IntermediateContextResult<TNext>(generated(), generated, this);
    }
}