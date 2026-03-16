public sealed class SpawnPositionSetCommandHandler : ICommandHandler
{
    public bool CanHandle(CommandData command) => command is SpawnPositionSetCommandData;

    public void Handle(CommandData command, CommandExecutionContext context)
    {
        context.TransitionRequester.RequestTransition(Mode.SetSpawnPoint);
    }
}