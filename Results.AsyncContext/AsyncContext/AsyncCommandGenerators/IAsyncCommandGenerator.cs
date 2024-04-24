using Results.AsyncContext.AsyncContext.AsyncCommands;

namespace Results.AsyncContext.AsyncContext.AsyncCommandGenerators;

public interface IAsyncCommandGenerator {
    IAsyncCommand Generate();
}