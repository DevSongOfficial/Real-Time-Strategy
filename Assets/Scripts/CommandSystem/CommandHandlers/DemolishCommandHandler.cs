public sealed class DemolishCommandHandler : ICommandHandler
{
    public bool CanHandle(CommandData command) => command is DemolishCommandData;

    public void Handle(CommandData command, CommandExecutionContext context)
    {
        var demolishCommand = (DemolishCommandData)command;

        context.CurrentEntity.ExecuteCommand(demolishCommand);
    }
}