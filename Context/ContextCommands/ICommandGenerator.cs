using Context.CallableGenerators;
using Context.ContextCallables;

namespace Context.ContextCommands;

public interface ICommandGenerator : ICallableGenerator {
    ICallable ICallableGenerator.Generate() {
        return Generate();
    }

    new ICommand Generate();
}