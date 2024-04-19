using Results.CallableGenerators;
namespace Results.ContextResultExtensions;

internal sealed class CallableCommandGenerator : ICommandGenerator {
    private readonly ICallableGenerator _callableGenerator;
    private readonly ICallableGenerator _undoGenerator;
    public CallableCommandGenerator(ICallableGenerator callableGenerator, ICallableGenerator undoGenerator) {
        _callableGenerator = callableGenerator;
        _undoGenerator = undoGenerator;
    }
    public ICommand Generate() => new CallableWrapCommand(_callableGenerator.Generate(), _undoGenerator.Generate());
}

internal sealed class CommandWrapper : ICommandGenerator {
    private readonly ICommand _command;
    
    public CommandWrapper(ICommand command) {
        _command = command;
    }
    public ICommand Generate() => _command;
}