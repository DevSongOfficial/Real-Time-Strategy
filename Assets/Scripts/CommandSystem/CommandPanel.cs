using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public sealed class CommandPanel : MonoBehaviour
{
    public event Action                     OnCommandButtonClicked;
    public event Action<BuildingData>       OnBuildingConstructionButtonClicked;

    public static readonly int ButtonCount = 12;
    [SerializeField] private CommandButton[] commandButtons = new CommandButton[ButtonCount];

    private IModeTransitionRequester transitionRequester;
    private TeamContext teamContext;

    private ISelectable currentEntity;
    private List<CommandData> currentCommands;

    private CommandExecutor commandExecutor;

    private void Awake()
    {
        commandExecutor = new CommandExecutor();

        foreach (var button in commandButtons)
            button.Setup(HandleCommandButtonClick);
    }

    public void Setup(IModeTransitionRequester transitionRequester, TeamContext teamContext)
    {
        this.transitionRequester = transitionRequester;
        this.teamContext = teamContext;
    }

    public void OnEntitySelected(ISelectable selectable)
    {
        currentEntity = selectable;
        currentCommands = currentEntity.GetData().CommandSet;

        if (selectable.GetTeam() == teamContext.Team)
            RefreshCommandButtons();
        else
            DisableAllButtons();
    }

    public void HandleCommandButtonClick(CommandData commandRequested)
    {
        var context = new CommandExecutionContext(
            currentEntity,
            transitionRequester,
            onBuildingConstructionButtonClicked:  buildingData => OnBuildingConstructionButtonClicked?.Invoke(buildingData)
        );

        var succeeded = commandExecutor.TryExecute(commandRequested, context);
        if(succeeded) OnCommandButtonClicked?.Invoke();
    }

    public void DisableAllButtons()
    {
        foreach (var button in commandButtons)
            button.Disable();
    }

    private void RefreshCommandButtons()
    {
        for (int i = 0; i < ButtonCount; i++)
        {
            var button = commandButtons[i];
            if (currentCommands.Count <= i)
            {
                button.Disable();
                continue;
            }

            var command = currentCommands[i];
            button.Refresh(command);
        }
    }
}