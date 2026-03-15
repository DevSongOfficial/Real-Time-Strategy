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

    private void Awake()
    {
        foreach (var button in commandButtons)
            button.Setup(this);
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

        Debug.Log(currentEntity);


        if (selectable.GetTeam() == teamContext.Team)
            RefreshCommandButtons();
        else
            DisableAllButtons();
    }

    public void HandleCommandButtonClick(CommandData commandRequested)
    {
        OnCommandButtonClicked?.Invoke();

        switch (commandRequested)
        {
            case BuildCommandData command:
                OnBuildingConstructionButtonClicked?.Invoke(command.BuildingData);
                transitionRequester.RequestTransition(Mode.Build);
                currentEntity.ExecuteCommand(command);
                break;
            case UnitTrainCommandData command:
                currentEntity.ExecuteCommand(command);
                break;
            case DemolishCommandData command:
                currentEntity.ExecuteCommand(command);
                break;
            case SpawnPositionSetCommandData:
                transitionRequester.RequestTransition(Mode.SetSpawnPoint);
                break;
            case SelectTargetCommandData:
                transitionRequester.RequestTransition(Mode.SelectTarget);
                break;
        }
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