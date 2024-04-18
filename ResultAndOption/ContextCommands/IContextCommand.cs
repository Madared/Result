namespace Results.ContextCommands;

public interface IContextCommand {
    Result Do();
    Result Undo();
}