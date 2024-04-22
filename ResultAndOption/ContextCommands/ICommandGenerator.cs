using Results.CallableGenerators;

namespace Results.ContextResultExtensions;

public interface ICommandGenerator : ICallableGenerator {
    ICallable ICallableGenerator.Generate() {
        return Generate();
    }

    ICommand Generate();
}