using System;
using UnityEngine;


public interface IPlacementView
{
    void ToggleUIPreview(bool enable);
    void ToggleBuildingPreview(bool enable, BuildingData selectedBuilding = null);
    void SetMouseIndicatorPosition(Vector3 position);
    void SetCellPosition(Vector3 position);
    void SetBuildingPreviewPosition(Vector3 position);
}

public enum PlacementMode { Idle, Placing }

public sealed class PlacementPresenter
{
    private readonly IPlacementView placementView;
    private readonly CommandPanel commandPanel;
    private readonly BuildingFactory buildingFactory;
    private readonly GridSystem gridSystem;

    private BuildingData selectedBuildingData; // Building to place.

    private PlacementMode placementMode;

    // Placement Info
    private Vector3 snappedPosition;
    private Quaternion rotation;

    public event Action OnPlacementFinished;

    public PlacementPresenter(IPlacementView placementView, CommandPanel commandPanel, BuildingFactory buildingFactory, GridSystem gridSystem)
    {
        this.placementView = placementView;
        this.buildingFactory = buildingFactory;
        this.commandPanel = commandPanel;
        this.gridSystem = gridSystem;
    }

    public void Enter()
    {

    }

    public void Exit()
    {
        Cancel();


    }

    public void UpdatePreview(Vector3 mouseWorld)
    {
        if (placementMode != PlacementMode.Placing) return;

        placementView.SetMouseIndicatorPosition(mouseWorld);

        var mouseCell3 = gridSystem.WorldToCell(mouseWorld);
        Vector2Int mouseCell = mouseCell3.ToVector2Int();

        Vector3 mouseCellWorld = gridSystem.CellToWorld(mouseCell3);
        placementView.SetCellPosition(mouseCellWorld);

        if (selectedBuildingData == null) return;

        Vector2Int size = selectedBuildingData.CellSize;

        Vector2Int originCell = gridSystem.MouseToOrigin(mouseCell, size);

        snappedPosition = gridSystem.GetFootprintPivotWorld(originCell, size);
        placementView.SetBuildingPreviewPosition(snappedPosition);

        gridSystem.DrawFootprintCells(mouseCell, size);
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
        Vector2Int cellSize = selectedBuildingData.CellSize;
        if (!gridSystem.CanPlace(cellPosition, cellSize)) return;
        

        placementMode = PlacementMode.Idle;

        // Setup Building.
        var building = buildingFactory.Create(selectedBuildingData, Player.Team);
        building.SetPosition(snappedPosition);


        // Add to Grid.
        gridSystem.Occupy(cellPosition, cellSize);

        placementView.ToggleUIPreview(false);
        placementView.ToggleBuildingPreview(false);

        selectedBuildingData = null;

        OnPlacementFinished?.Invoke();
    }

    public void Cancel()
    {
        if (selectedBuildingData == null) return;

        placementMode = PlacementMode.Idle;

        placementView.ToggleUIPreview(false);
        placementView.ToggleBuildingPreview(false);

        selectedBuildingData = null;

        OnPlacementFinished?.Invoke();
    }
}