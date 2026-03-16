public sealed class BuildCommandHandler : ICommandHandler
{
    public bool CanHandle(CommandData command) => command is BuildCommandData;

    public void Handle(CommandData command, CommandExecutionContext context)
    {
        var buildCommand = (BuildCommandData)command;

        context.OnBuildingConstructionButtonClicked?.Invoke(buildCommand.BuildingData);
        context.TransitionRequester.RequestTransition(Mode.Build);
        context.CurrentEntity.ExecuteCommand(buildCommand);
    }
}