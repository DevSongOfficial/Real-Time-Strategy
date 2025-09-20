using System;
using UnityEngine;
using static CustomResourceManagement.Prefabs.Playable;


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
    private readonly GridSystem gridSystem;

    private BuildingData selectedBuildingData; // Building to place.

    private PlacementMode placementMode;

    // Placement Info
    private Vector3 snappedPosition;
    private Quaternion rotation;

    public PlacementPresenter(IPlacementView placementView, BuildingFactory buildingFactory, GridSystem gridSysetm)
    {
        this.placementView = placementView;
        this.buildingFactory = buildingFactory;
        this.gridSystem = gridSysetm;
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
        var cellPosition = gridSystem.WorldToCell(mouseWorld);
        snappedPosition = gridSystem.CellToWorld(cellPosition);
        placementView.SetCellPosition(snappedPosition);

        // Set preview position.
        placementView.SetBuildingPreviewPosition(snappedPosition);


        gridSystem.DrawFootprintCells(cellPosition.ToVector2Int(), Vector2Int.one);
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

        // Check on Grid.
        Vector2Int cellPosition = gridSystem.WorldToCell(snappedPosition).ToVector2Int();
        Vector2Int cellSize = Vector2Int.one * selectedBuildingData.RadiusOnTerrain * 2;
        if (!gridSystem.CanPlace(cellPosition, cellSize)) return;

        placementMode = PlacementMode.Idle;

        // Setup Building.
        var building = buildingFactory.Create(selectedBuildingData);
        building.SetPosition(snappedPosition);

        // Add to Grid.
        gridSystem.Occupy(cellPosition, cellSize);

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