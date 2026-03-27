using CustomResourceManagement;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapEditorUI : MonoBehaviour
{
    [SerializeField]    private MapEditor editor;
    [Space]

    [Header("UI Refs")]
    [SerializeField]    private Transform spawnPanel;
                        private List<EditorEntitySpawnButton> spawnButtons;

    private void Start()
    {
        // Generate entity spawn buttons from building data DB
        spawnButtons = new();
        var buttonPrefab = ResourceLoader.GetResource<EditorEntitySpawnButton>(Prefabs.UI.EditorEntitySpawnButton);
        foreach (var buildingData in editor.EntityDataDB.GetBuildingDatas())
        {
            var button = GameObject.Instantiate(buttonPrefab, spawnPanel);
            button.Setup(editor, buildingData);
            spawnButtons.Add(button);
        }
    }
}
