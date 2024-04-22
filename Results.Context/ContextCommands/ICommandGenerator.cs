using Results.Context.CallableGenerators;
using Results.Context.ContextCallables;

namespace Results.Context.ContextCommands;

public interface ICommandGenerator : ICallableGenerator {
    ICallable ICallableGenerator.Generate() {
        return Generate();
    }

    new ICommand Generate();
}