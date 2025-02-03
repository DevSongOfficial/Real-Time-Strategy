using System.Collections.Generic;
using UnityEngine;
using CustomResourceManagement;

public sealed class UnitController : MonoBehaviour
{
    // Temporary for testing.
    [SerializeField, Range(0, 99)] private int numberOfUnitOnStart = 3;
    private void GenerateUnits(int numberOfUnit)
    {
        for (int i = 0; i < numberOfUnit; i++)
        {
            // Generate Units.
            var prefab_Unit = ResourceLoader.GetResource<Unit>(Prefabs.Playable.Unit.Unit_1);
            var randomPosition = new Vector3(Random.Range(26, 35), 2, Random.Range(20, 36));
            var newUnit = Instantiate(prefab_Unit, randomPosition, Quaternion.identity);
            allUnits.Add(newUnit);

            var prefab_HealthBar = ResourceLoader.GetResource<HealthTracker>(Prefabs.UI.HealthTracker);
            var newHealthBar = Instantiate(prefab_HealthBar, canvas.transform);
            newHealthBar.SetUp(camera, new Target(newUnit));
        }
    }

    [SerializeField] private new Camera camera;
    [SerializeField] private Canvas canvas;

    // Temporary for testing.
    private List<ISelectable> allUnits;

    // Mouse drag event.
    private DragEventHandler dragEventHandler;

    // Playbles selection.
    private SelectionHandler selectionHandler;
    private List<ISelectable> selectedUnits;
    [SerializeField] private LayerMask layerMask_Selectable;



    private void Awake()
    {
        selectedUnits = new List<ISelectable>();
        allUnits = new List<ISelectable>();

        
        dragEventHandler    = new DragEventHandler(allUnits.FilterByType<ISelectable, ITransformProvider>(), camera, canvas);
        selectionHandler    = new SelectionHandler(selectedUnits, camera, layerMask_Selectable);

        dragEventHandler.OnUnitDetectedInDragArea += selectionHandler.SelectUnits;
    }

    private void Start()
    {
        GenerateUnits(numberOfUnitOnStart);
    }

    private void Update()
    {
        dragEventHandler.HandleDragEvent();
        selectionHandler.HandleUnitSelection();
        selectionHandler.HandleTargetSelection();
    }
}