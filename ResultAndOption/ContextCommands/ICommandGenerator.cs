using ResultAndOption.CallableGenerators;
using ResultAndOption.ContextCallables;

namespace ResultAndOption.ContextCommands;

public interface ICommandGenerator : ICallableGenerator {
    ICallable ICallableGenerator.Generate() {
        return Generate();
    }

    new ICommand Generate();
}