using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class CommandPanel : MonoBehaviour
{
    public static readonly int ButtonCount = 12;
    [SerializeField] private CommandButton[] commandButtons = new CommandButton[ButtonCount];

    public event Action OnCommandButtonClicked;
    public event Action<BuildingData> OnBuildingConstructionButtonClicked;
    public event Action<UnitData> OnUnitTrainButtonClicked;


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

    public void HandleCommandButtonClick(CommandData command)
    {
        OnCommandButtonClicked?.Invoke();

        switch (command)
        {
            case BuildCommandData buildCommand:
                OnBuildingConstructionButtonClicked?.Invoke(buildCommand.BuildingData);
                currentEntity.ExecuteCommand(command);
                break;
            case UnitTrainCommandData unitTrainCommand:
                OnUnitTrainButtonClicked?.Invoke(unitTrainCommand.UnitData);
                currentEntity.ExecuteCommand(command);
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