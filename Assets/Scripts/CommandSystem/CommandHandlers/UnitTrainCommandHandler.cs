public sealed class UnitTrainCommandHandler : ICommandHandler
{
    public bool CanHandle(CommandData command) => command is UnitTrainCommandData;

    public void Handle(CommandData command, CommandExecutionContext context)
    {
        var trainCommand = (UnitTrainCommandData)command;

        context.CurrentEntity.ExecuteCommand(trainCommand);
    }
}