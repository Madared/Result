using Results.CallableGenerators;

namespace Results.ContextResultExtensions;

public interface ICommandGenerator : ICallableGenerator {
    IContextCallable ICallableGenerator.Generate() {
        return Generate();
    }

    ICommand Generate();
}