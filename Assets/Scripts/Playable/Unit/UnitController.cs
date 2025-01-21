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

    [SerializeField] private LayerMask layerMask_Ground;
    [SerializeField] private LayerMask layerMask_Selectable;


    // Unit Selection
    private List<ISelectable> selectedUnits;
    private List<ISelectable> allUnits;
    public IEnumerable<ISelectable> EnumerateAllUnits()
    {
        foreach (var unit in allUnits)
            yield return unit;

    }

    // Handle mouse drag events.
    private DragEventHandler dragEventHandler;

    private void Awake()
    {
        selectedUnits = new List<ISelectable>();
        allUnits = new List<ISelectable>();

        //Setup components.
        dragEventHandler = new DragEventHandler(camera, canvas, this);
        dragEventHandler.OnUnitDetectedInDragArea += SelectUnits;
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
        dragEventHandler.Update();

        HandleUnitSelection();
        HandleUnitMovement();
    }

    private void HandleUnitSelection()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        
        // Allows mutil-selection.
        if(!Input.GetKey(KeyCode.LeftShift))
            DeselectAllUnits();

        var ray = camera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask_Selectable)) return;

        if (hit.collider.TryGetComponent(out ISelectable unit))
        {
            SelectUnit(unit);
        }
    }

    private void SelectUnit(ISelectable unit)
    {
        if(!selectedUnits.Contains(unit)) 
            selectedUnits.Add(unit);

        unit.OnSelected();
    }

    private void SelectUnits(List<ISelectable> units)
    {
        foreach (var unit in units)
            SelectUnit(unit);

    }

    private void DeselectUnit(ISelectable unit)
    {
        if (selectedUnits.Contains(unit))
            selectedUnits.Remove(unit);

        unit.OnDeselected();
    }

    private void DeselectAllUnits()
    {
        for(int i = selectedUnits.Count - 1; i >= 0; i--)
            DeselectUnit(selectedUnits[i]);
    }

    private void HandleUnitMovement()
    {
        if (!Input.GetMouseButtonDown(1)) return;
        if (selectedUnits.Count == 0) return;

        var ray = camera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask_Ground)) return;

        foreach (var unit in selectedUnits)
            MoveUnit(unit, hit.point);

    }

    private void MoveUnit(ISelectable unit, Vector3 destination)
    {
        if (unit is IMovable movable)
        {
            movable.MoveTo(destination);
        }
    }
}