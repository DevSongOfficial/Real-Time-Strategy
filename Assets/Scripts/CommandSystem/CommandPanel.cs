using System;
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
    private CommandSetData currentCommandSet;

    private void Awake()
    {
        foreach (var button in commandButtons)
            button.Setup(this);
    }

    public void OnEntitySelected(ISelectable selectable)
    {
        currentEntity = selectable;
        currentCommandSet = currentEntity.CommandSet;

        if (selectable.GetTeam() == Player.Team)
            RefreshCommandButtons();
        else
            DisableAllButtons();
    }

    public void HandleCommandButtonClick(Command command)
    {
        OnCommandButtonClicked?.Invoke();

        var entityData = command.entityToGenerate;

        if (entityData is UnitData unitData)
            OnUnitTrainButtonClicked?.Invoke(unitData);
        else if (entityData is BuildingData buildingData)
            OnBuildingConstructionButtonClicked?.Invoke(buildingData);

        currentEntity.ExecuteCommand(command);
    }

    private void RefreshCommandButtons()
    {
        for (int i = 0; i < ButtonCount; i++)
        {
            var button = commandButtons[i];
            if (currentCommandSet.Commands.Count <= i)
            {
                button.Disable();
                continue;
            }

            var command = currentCommandSet.Commands[i];
            button.Refresh(command);
        }
    }

    private void DisableAllButtons()
    {
        foreach (var button in commandButtons)
            button.Disable();
    }
}