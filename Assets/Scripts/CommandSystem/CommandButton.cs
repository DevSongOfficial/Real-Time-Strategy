using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CommandButton : MonoBehaviour
{
    private CommandPanel commandPanel;
    private CommandData command; // current command

    [SerializeField] private Button button;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI textMeshPro;

    public void Setup(CommandPanel commandPanel)
    {
        this.commandPanel = commandPanel;

        if(button == null)
            button = GetComponent<Button>();
        
        if(image == null)
            image = GetComponent<Image>();
    }

    public void Refresh(CommandData command)
    {
        this.command = command;

        image.sprite = command.Icon;
        textMeshPro.text = command.Tooltip;

        button.onClick.RemoveAllListeners();
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
        commandPanel.HandleCommandButtonClick(command);
    }
}