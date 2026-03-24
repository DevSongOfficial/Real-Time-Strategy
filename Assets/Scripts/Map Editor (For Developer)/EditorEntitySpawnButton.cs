using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EditorEntitySpawnButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text buttonText;

    private MapEditor editor;
    private EntityData entityData;

    public void Setup(MapEditor editor, EntityData data)
    {
        this.editor = editor;
        this.entityData = data;

        if (button == null)
            button = GetComponent<Button>();

        if(buttonText == null)
            buttonText = GetComponentInChildren<TMP_Text>();

        buttonText.text = data.DisplayName;

        button.onClick.AddListener(HandleClick);
    }

    private void HandleClick()
    {
        editor.AddBuilding(entityData as BuildingData);
    }
}