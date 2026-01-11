using System;
using Unity.VisualScripting;
using UnityEngine;

public class CommandPanel : MonoBehaviour
{
    public static readonly int ButtonCount = 12;
    [SerializeField] private CommandButton[] commandButtons = new CommandButton[ButtonCount];

    public Action OnCommandButtonClicked;
    public Action<BuildingData> OnBuildingButtonClicked;

    private CommandSetData currentCommandSet;

    private void Awake()
    {
        foreach (var button in commandButtons)
            button.Setup(this);
    }

    public void OnEntitySelected(ISelectable selectable)
    {
        currentCommandSet = selectable.CommandSet;

        for(int i = 0; i < ButtonCount;  i++)
        {
            var button = commandButtons[i];
            if(currentCommandSet.Commands.Count <= i)
            {
                button.Disable();
                continue;
            }

            var command = currentCommandSet.Commands[i];
            button.Refresh(command.type, command.icon, command.tooltip, command.entityToGenerate);
        }
    }
    
}