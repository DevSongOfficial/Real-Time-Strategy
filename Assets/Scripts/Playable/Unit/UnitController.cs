using System.Collections.Generic;
using UnityEngine;
using CustomResourceManagement;

public interface IUnitSelector
{
    public IEnumerable<ISelectable> EnumerateAllUnits();
}

public sealed class UnitController : MonoBehaviour, IUnitSelector
{
    [SerializeField] private new Camera camera;
    [SerializeField] private Canvas canvas;

    private List<ISelectable> allUnits;

    // Mouse Drag Event
    private DragEventHandler dragEventHandler;

    // Unit Selection
    private SelectionHandler selectionHandler;
    [SerializeField] private LayerMask layerMask_Selectable;
    private List<ISelectable> selectedUnits;

    // Unit Movement
    private UnitMovementHandler unitMovementHandler;
    [SerializeField] private LayerMask layerMask_Ground;

    private void Awake()
    {
        selectedUnits = new List<ISelectable>();
        allUnits = new List<ISelectable>();

        
        dragEventHandler    = new DragEventHandler(camera, canvas, this as IUnitSelector);
        selectionHandler    = new SelectionHandler(selectedUnits, camera, layerMask_Selectable);
        unitMovementHandler = new UnitMovementHandler(selectedUnits, camera, layerMask_Ground);

        dragEventHandler.OnUnitDetectedInDragArea += selectionHandler.SelectUnits;
    }

    private void Start()
    {
        // Temporary
        for (int i = 0; i < 3 ; i++)
        {
            var prefab = ResourceLoader.GetResource<Unit>(Prefabs.Playable.Unit.Unit_1);
            var randomPosition = new Vector3(Random.Range(26, 35), 2, Random.Range(20, 36));
            allUnits.Add(Instantiate(prefab, randomPosition, Quaternion.identity));
        }
    }

    private void Update()
    {
        dragEventHandler.HandleDragEvent();
        selectionHandler.HandleUnitSelection();
        unitMovementHandler.HandleUnitMovement();
    }

    public IEnumerable<ISelectable> EnumerateAllUnits()
    {
        foreach (var unit in allUnits)
            yield return unit;

    }
}