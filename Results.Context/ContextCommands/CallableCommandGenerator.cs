using Results.Context.ActionCallables;
using Results.Context.CallableGenerators;

namespace Results.Context.ContextCommands;

internal sealed class CallableCommandGenerator : ICommandGenerator {
    private readonly ICallableGenerator _callableGenerator;
    private readonly IActionCallableGenerator _undoGenerator;

    public CallableCommandGenerator(ICallableGenerator callableGenerator, IActionCallableGenerator undoGenerator) {
        _callableGenerator = callableGenerator;
        _undoGenerator = undoGenerator;
    }

    public ICommand Generate() {
        return new CallableWrapCommand(_callableGenerator.Generate(), _undoGenerator.Generate());
    }
}

internal sealed class CommandWrapper : ICommandGenerator {
    private readonly ICommand _command;

    public CommandWrapper(ICommand command) {
        _command = command;
    }

    public ICommand Generate() {
        return _command;
    }
}