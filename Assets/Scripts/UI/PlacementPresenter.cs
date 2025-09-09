using System;
using UnityEngine;


public interface IPlacementView
{
    void TogglePreview(bool enable, Transform prefabToPreview = null);
    void ToggleButtonPanel(bool enable);
    void SetMouseIndicatorPosition(Vector3 position);
    void SetCellPosition(Vector3 position);
    void SetBuildingIndicatorPosition(Vector3 position);
    void Place(Transform prefab);

    // View -> Presenter
    event Action<Transform> OnPrefabSelected;
}

public enum PlacementMode { Idle, Placing }

public class PlacementPresenter
{
    private readonly IPlacementView view;
    private Transform selectedPrefab;
    private readonly Grid grid;

    private PlacementMode placementMode;

    public PlacementPresenter(IPlacementView view, Grid grid)
    {
        this.view = view;
        this.grid = grid;

        view.OnPrefabSelected += SelectPrefab;
    }

    public void Enter()
    {
        view.ToggleButtonPanel(true);
    }

    public void Exit()
    {
        view.ToggleButtonPanel(false);
    }

    public void UpdatePreview(Vector3 mouseWorld)
    {
        if (placementMode != PlacementMode.Placing) return;

        // Set mouse indicator position.
        view.SetMouseIndicatorPosition(mouseWorld);

        // Set cell indicator position.
        var cell = grid.WorldToCell(mouseWorld);
        var snapped = grid.CellToWorld(cell);
        view.SetCellPosition(snapped.WithY(0));

        view.SetBuildingIndicatorPosition(mouseWorld);
    }

    public void SelectPrefab(Transform prefab)
    {
        selectedPrefab = prefab;
        view.TogglePreview(true, prefab); // todo: Is it better to put this in view(PlacementSystem)?
        placementMode = PlacementMode.Placing;
    }

    public void Place()
    {
        if (selectedPrefab == null) return;

        view.Place(selectedPrefab);
        view.TogglePreview(false);

        selectedPrefab = null;
        placementMode = PlacementMode.Idle;
    }

    public void Cancel()
    {
        if (selectedPrefab == null) return;

        view.TogglePreview(false);

        selectedPrefab = null;
        placementMode = PlacementMode.Idle;
    }
}
