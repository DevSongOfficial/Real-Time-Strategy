using System;
using UnityEngine;


public interface IPlacementView
{
    void TogglePreview(bool enable);
    void ToggleButtonPanel(bool enable);
    void SetMouseIndicatorPosition(Vector3 position);
    void SetCellPosition(Vector3 position);
    void Place(Transform prefab);

    // View -> Presenter
    event Action<Transform> OnPrefabSelected;
}

public class PlacementPresenter
{
    private readonly IPlacementView view;
    private Transform selectedPrefab;
    private readonly Grid grid;

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
        // Set mouse indicator position.
        view.SetMouseIndicatorPosition(mouseWorld);

        // Set cell indicator position.
        var cell = grid.WorldToCell(mouseWorld);
        var snapped = grid.CellToWorld(cell);
        view.SetCellPosition(new Vector3(snapped.x, 0, snapped.z));
        //view.SetCellIndicator(snapped.WithY(0));
    }

    public void SelectPrefab(Transform prefab)
    {
        selectedPrefab = prefab;
        view.TogglePreview(true); // todo: Is it better to put this in view(PlacementSystem)?
    }

    public void Place()
    {
        if (selectedPrefab == null) return;
        view.Place(selectedPrefab);
        view.TogglePreview(false);
    }
}
