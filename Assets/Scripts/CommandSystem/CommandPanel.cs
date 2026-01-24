using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class CommandPanel : MonoBehaviour
{
    public static readonly int ButtonCount = 12;
    [SerializeField] private CommandButton[] commandButtons = new CommandButton[ButtonCount];

    public event Action                     OnCommandButtonClicked;
    public event Action<BuildingData>       OnBuildingConstructionButtonClicked;
    public event Action<UnitData>           OnUnitTrainButtonClicked;
    public event Action<IUnitGenerator>     OnSpawnPositionSetButtonClicked;
    public event Action                     OnSelectTargetButtonClicked ;


    private ISelectable currentEntity;
    private List<CommandData> currentCommands;

    private void Awake()
    {
        foreach (var button in commandButtons)
            button.Setup(this);
    }

    public void OnEntitySelected(ISelectable selectable)
    {
        currentEntity = selectable;
        currentCommands = currentEntity.GetData().CommandSet;

        if (selectable.GetTeam() == Player.Team)
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
                currentEntity.ExecuteCommand(command);
                break;
            case UnitTrainCommandData command:
                OnUnitTrainButtonClicked?.Invoke(command.UnitData);
                currentEntity.ExecuteCommand(command);
                break;
            case SpawnPositionSetCommandData:
                OnSpawnPositionSetButtonClicked?.Invoke(currentEntity as IUnitGenerator);
                break;
            case SelectTargetCommandData:
                OnSelectTargetButtonClicked?.Invoke();
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