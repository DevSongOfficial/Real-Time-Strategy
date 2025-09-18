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
    private readonly BuildingFactory buildingFactory;

    private BuildingData selectedBuildingData; // Building to place.

    private PlacementMode placementMode;

    // Placement Info
    private Vector3 snappedPosition;
    private Quaternion rotation;

    public PlacementPresenter(IPlacementView placementView, BuildingFactory buildingFactory)
    {
        this.placementView = placementView;
        this.buildingFactory = buildingFactory;
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
        snappedPosition = GridController.CellToWorld(cellPosition);
        placementView.SetCellPosition(snappedPosition);

        // Set preview position.
        placementView.SetBuildingPreviewPosition(snappedPosition);
    }

    public void SelectBuilding(BuildingData data)
    {
        placementMode = PlacementMode.Placing;

        selectedBuildingData = data;

        // Preview building prefab & related UI
        placementView.ToggleBuildingPreview(true, selectedBuildingData);
        placementView.ToggleUIPreview(true);
    }

    public void Place()
    {
        if (selectedBuildingData == null) return;

        placementMode = PlacementMode.Idle;

        // Setup Building.
        var building = buildingFactory.Create(selectedBuildingData);
        building.SetPosition(snappedPosition);

        placementView.ToggleUIPreview(false);
        placementView.ToggleBuildingPreview(false);

        selectedBuildingData = null;
    }

    public void Cancel()
    {
        if (selectedBuildingData == null) return;

        placementMode = PlacementMode.Idle;

        placementView.ToggleUIPreview(false);
        placementView.ToggleBuildingPreview(false);

        selectedBuildingData = null;
    }
}