using System.Collections.Generic;
using UnityEngine;
using CustomResourceManagement;

public sealed class UnitController : MonoBehaviour
{
    [SerializeField] private new Camera camera;

    [SerializeField] private LayerMask layerMask_Ground;
    [SerializeField] private LayerMask layerMask_Selectable;


    private List<ISelectable> selectedUnits;
    private List<ISelectable> allUnits;

    private void Awake()
    {
        selectedUnits = new List<ISelectable>();
        allUnits = new List<ISelectable>();
    }

    private void Start()
    {
        // Temporary
        for (int i = 0; i < 3 ; i++)
        {
            var prefab = ResourceLoader.GetResource<Unit>(Prefabs.Playable.Unit.Unit_1);
            allUnits.Add(Instantiate(prefab));
        }
    }

    private void Update()
    {
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
        {
            MoveUnit(unit, hit.point);
        }
    }

    private void MoveUnit(ISelectable unit, Vector3 destination)
    {
        if (unit is IMovable movable)
        {
            movable.MoveTo(destination);
        }
    }
}