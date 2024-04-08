using System.Net.Http.Headers;

namespace Results;

public delegate Result<TOut> ResultGetter<TOut>() where TOut : notnull;

public delegate Result<TOut> InputToResult<in TIn, TOut>(TIn input) where TIn : notnull where TOut : notnull;

public delegate Result SimpleResultGetter();

public delegate Result InputToSimpleResult<in TIn>(TIn input) where TIn : notnull;

public class ContextResult<TIn, TOut> : IContextResult<TOut> where TIn : notnull where TOut : notnull {
    private readonly IContextResult<TIn> _previousContext;
    private readonly Func<TIn, Result<TOut>> _called;
    private readonly Result<TOut> _result;

    public IError Error => _result.Error;
    public TOut Data => _result.Data;
    public bool Succeeded => _result.Succeeded;
    public bool Failed => _result.Failed;

    public ContextResult(Func<TIn, Result<TOut>> called, IContextResult<TIn> previousContext, Result<TOut> result) {
        _called = called;
        _previousContext = previousContext;
        _result = result;
    }

    IContextResult<TOut> IContextResult<TOut>.Retry() => Retry();


    public Result<TOut> StripContext() => Succeeded ? Result<TOut>.Ok(Data) : Result<TOut>.Fail(Error);

    public IContextResult<TNext> Map<TNext>(Func<TOut, Result<TNext>> mapper) where TNext : notnull {
        throw new NotImplementedException();
    }

    public IContextResult<TNext> Map<TNext>(Func<TOut, TNext> mapper) where TNext : notnull {
        throw new NotImplementedException();
    }

    public IContextResult Map(Func<TOut, Result> mapper) {
        throw new NotImplementedException();
    }

    public IContextResult Map(Action<TOut> mapper) {
        throw new NotImplementedException();
    }

    public IContextResult Map(Action action) {
        throw new NotImplementedException();
    }

    public IContextResult Map(Func<Result> mapper) {
        throw new NotImplementedException();
    }

    public IContextResult<TOut1> Map<TOut1>(Func<Result<TOut1>> mapper) where TOut1 : notnull {
        throw new NotImplementedException();
    }

    public IContextResult<TOut1> Map<TOut1>(Func<TOut1> mapper) where TOut1 : notnull {
        throw new NotImplementedException();
    }

    public ContextResult<TIn, TOut> Retry() {
        if (Succeeded) return this;
        IContextResult<TIn> previousContext = _previousContext.Retry();
        return previousContext.Failed
            ? new ContextResult<TIn, TOut>(_called, previousContext, Result<TOut>.Fail(previousContext.Error))
            : new ContextResult<TIn, TOut>(_called, previousContext, _called(previousContext.Data));
    }
}