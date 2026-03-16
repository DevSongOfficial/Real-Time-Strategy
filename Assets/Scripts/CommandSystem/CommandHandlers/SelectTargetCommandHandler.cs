public sealed class SelectTargetCommandHandler : ICommandHandler
{
    public bool CanHandle(CommandData command) => command is SelectTargetCommandData;

    public void Handle(CommandData command, CommandExecutionContext context)
    {
        context.TransitionRequester.RequestTransition(Mode.SelectTarget);
    }
}