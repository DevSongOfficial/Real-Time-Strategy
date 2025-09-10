using System;
using UnityEngine;


public interface IPlacementView
{
    void ToggleUIPreview(bool enable);
    void ToggleButtonPanel(bool enable);
    void ToggleBuildingPreview(bool enable, BuildingData selectedBuilding = null);
    void SetMouseIndicatorPosition(Vector3 position);
    void SetCellPosition(Vector3 position);
    void SetBuildingPreviewPosition(Vector3 position);

    // View -> Presenter
    event Action<BuildingData> OnBuildingSelected;
}

public enum PlacementMode { Idle, Placing }

public sealed class PlacementPresenter
{
    private readonly IPlacementView placementView;
    private readonly BuildingFactory factory;

    private BuildingData selectedBuilding; // Building to place.

    private PlacementMode placementMode;

    // Placement Info
    private Vector3 snappedPosition;
    private Quaternion rotation;

    public PlacementPresenter(IPlacementView placementView, BuildingFactory factory)
    {
        this.placementView = placementView;
        this.factory = factory;
    }

    public void Enter()
    {
        placementView.ToggleButtonPanel(true);
        placementView.OnBuildingSelected += SelectBuilding;
    }

    public void Exit()
    {
        placementView.ToggleButtonPanel(false);
        Cancel();
        placementView.OnBuildingSelected -= SelectBuilding;
    }

    public void UpdatePreview(Vector3 mouseWorld)
    {
        if (placementMode != PlacementMode.Placing) return;

        // Set mouse indicator position.
        placementView.SetMouseIndicatorPosition(mouseWorld);

        // Set cell indicator position.
        var cellPosition = GridController.WorldToCell(mouseWorld);
        snappedPosition = GridController.CellToWorld(cellPosition).WithY(0);
        placementView.SetCellPosition(snappedPosition);

        // Set preview position.
        placementView.SetBuildingPreviewPosition(snappedPosition);
    }

    public void SelectBuilding(BuildingData data)
    {
        placementMode = PlacementMode.Placing;

        selectedBuilding = data;

        // Preview building prefab & related UI
        placementView.ToggleBuildingPreview(true, selectedBuilding);
        placementView.ToggleUIPreview(true);
    }

    public void Place()
    {
        if (selectedBuilding == null) return;

        placementMode = PlacementMode.Idle;

        var building = factory.Create(selectedBuilding);
        building.SetPosition(snappedPosition);

        placementView.ToggleUIPreview(false);
        placementView.ToggleBuildingPreview(false);

        selectedBuilding = null;
    }

    public void Cancel()
    {
        if (selectedBuilding == null) return;

        placementMode = PlacementMode.Idle;

        placementView.ToggleUIPreview(false);
        placementView.ToggleBuildingPreview(false);

        selectedBuilding = null;
    }
}