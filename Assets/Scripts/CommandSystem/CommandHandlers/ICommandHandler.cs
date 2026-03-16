public interface ICommandHandler
{
    bool CanHandle(CommandData command);
    void Handle(CommandData command, CommandExecutionContext context);
}