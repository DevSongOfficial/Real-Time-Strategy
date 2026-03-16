using System;
using System.Linq;
using System.Collections.Generic;

public sealed class CommandExecutor
{
    private readonly List<ICommandHandler> handlers;

    public CommandExecutor()
    {
        handlers = new List<ICommandHandler>
        {
            new BuildCommandHandler(),
            new UnitTrainCommandHandler(),
            new DemolishCommandHandler(),
            new SpawnPositionSetCommandHandler(),
            new SelectTargetCommandHandler()
        };
    }

    public bool TryExecute(CommandData command, CommandExecutionContext context)
    {
        var handler = FindHandler(command);
        if (handler == null) return false;

        handler.Handle(command, context);

        return true;
    }

    private ICommandHandler FindHandler(CommandData command)
    {
        foreach (var handler in handlers)
            if (handler.CanHandle(command))
                return handler;

        return null;
    }
}

public sealed class CommandExecutionContext
{
    public ISelectable CurrentEntity { get; }
    public IModeTransitionRequester TransitionRequester { get; }
    public Action<BuildingData> OnBuildingConstructionButtonClicked { get; }

    public CommandExecutionContext(
        ISelectable currentEntity,
        IModeTransitionRequester transitionRequester,
        Action<BuildingData> onBuildingConstructionButtonClicked)
    {
        CurrentEntity = currentEntity;
        TransitionRequester = transitionRequester;
        OnBuildingConstructionButtonClicked = onBuildingConstructionButtonClicked;
    }
}