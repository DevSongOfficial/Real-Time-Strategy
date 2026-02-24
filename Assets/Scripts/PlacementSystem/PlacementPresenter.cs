using System;
using UnityEngine;


public interface IPlacementView
{
    void ToggleUIPreview(bool enable);
    void ToggleBuildingPreview(bool enable, BuildingData selectedBuilding = null);
    void SetMouseIndicatorPosition(Vector3 position);
    void SetBuildingPreviewPosition(Vector3 position);
}

public interface IPlacementEvent
{
    event Action<ITarget> OnPlacementRequested; // ITarget: Requested Building that was initialized
    event Action<Vector3> OnPlacementCanceled;
}

public enum PlacementMode { Idle, Placing }

public sealed class PlacementPresenter : IPlacementEvent
{
    private readonly IPlacementView placementView;
    private readonly CommandPanel commandPanel;
    private readonly BuildingFactory buildingFactory;
    private readonly GridSystem gridSystem;
    private readonly InputManager inputManager;


    private PlacementMode placementMode;

    // Placement Info
    private Vector3 snappedPosition;
    private Quaternion rotation;
    private BuildingData buildingData; // Building to place.

    public event Action<ITarget> OnPlacementRequested; // ITarget: Requested Building that was initialized
    public event Action<Vector3> OnPlacementCanceled;

    public event Action<Building> OnBuildingDestroyed;
    public event Action<Building> OnBuildingDeconstructionRequested;

    public PlacementPresenter(IPlacementView placementView, CommandPanel commandPanel, BuildingFactory buildingFactory, GridSystem gridSystem, InputManager inputManager)
    {
        this.placementView = placementView;
        this.buildingFactory = buildingFactory;
        this.commandPanel = commandPanel;
        this.gridSystem = gridSystem;
        this.inputManager = inputManager;
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

        Vector3Int  mouseCell3      = gridSystem.WorldToCell(mouseWorld);
        Vector2Int  mouseCell       = mouseCell3.ToVector2Int();
        Vector3     mouseCellWorld  = gridSystem.CellToWorld(mouseCell3);

        if (buildingData == null) return;

        Vector2Int size = buildingData.CellSize;

        Vector2Int originCell = gridSystem.MouseToOrigin(mouseCell, size);

        snappedPosition = gridSystem.GetFootprintPivotWorld(originCell, size);
        placementView.SetBuildingPreviewPosition(snappedPosition);

        gridSystem.DrawFootprintCells(mouseCell, size);
    }

    public void SelectBuilding(BuildingData data)
    {
        placementMode = PlacementMode.Placing;

        buildingData = data;

        // Preview building prefab & related UI
        placementView.ToggleBuildingPreview(true, buildingData);
        placementView.ToggleUIPreview(true);
    }

    public bool TryPlace()
    {
        return TryPlace(buildingData, snappedPosition, Player.Team, out var building);
    }

    public bool TryPlace(BuildingData buildingData, Vector3 position, Team team, out Building placed)
    {
        placed = null;

        if (buildingData == null) return false;

        // Check on Grid.
        Vector2Int cellPosition = gridSystem.WorldToCell(position).ToVector2Int();
        Vector2Int cellSize = buildingData.CellSize;
        if (!gridSystem.CanPlace(cellPosition, cellSize)) return false;


        placementMode = PlacementMode.Idle;

        // Create & setup building.
        Building building = buildingFactory.Create(buildingData, team);
        building.SetPosition(position);
        building.OnDestroyed += OnBuildingDestroyed;
        building.OnDestructionRequested += OnBuildingDeconstructionRequested;
        placed = building;

        // Add to Grid.
        gridSystem.Occupy(cellPosition, cellSize);

        placementView.ToggleUIPreview(false);
        placementView.ToggleBuildingPreview(false);

        buildingData = null;

        OnPlacementRequested?.Invoke(building);

        return true;
    }


    public void Cancel()
    {
        if (buildingData == null) return;

        placementMode = PlacementMode.Idle;

        placementView.ToggleUIPreview(false);
        placementView.ToggleBuildingPreview(false);

        buildingData = null;

        OnPlacementCanceled?.Invoke(inputManager.GetMousePositionOnCanvas());
    }
}