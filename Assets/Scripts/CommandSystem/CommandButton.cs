using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CommandButton : MonoBehaviour
{
    private CommandPanel commandPanel;

    [SerializeField] private Button button;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI textMeshPro;

    // Current Button Info
    private CommandType commandType;
    private EntityData entityData; // entity to generate.

    public void Setup(CommandPanel commandPanel)
    {
        this.commandPanel = commandPanel;

        if(button == null)
            button = GetComponent<Button>();
        
        if(image == null)
            image = GetComponent<Image>();
    }

    public void Refresh(CommandType commandType, Sprite icon, string tooltip, EntityData entityToGenerate)
    {
        this.commandType = commandType;
        image.sprite = icon;
        textMeshPro.text = tooltip;
        entityData = entityToGenerate;

        button.onClick.AddListener(OnClicked);
    }

    public void Disable()
    {
        image.sprite = null;
        textMeshPro.text = "Not Assigned";

        button.onClick.RemoveListener(OnClicked);
    }

    private void OnClicked()
    {
        commandPanel.OnCommandButtonClicked?.Invoke();

        if(commandType == CommandType.TrainUnit)
        {
        }
        else if(commandType == CommandType.Build)
        {
            commandPanel.OnBuildingButtonClicked?.Invoke(entityData as BuildingData);
        }
    }
}