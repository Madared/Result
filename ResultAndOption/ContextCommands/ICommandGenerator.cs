using Results.CallableGenerators;

namespace Results.ContextResultExtensions;

public interface ICommandGenerator : ICallableGenerator {
    IResultCallable ICallableGenerator.Generate() {
        return Generate();
    }

    ICommand Generate();
}