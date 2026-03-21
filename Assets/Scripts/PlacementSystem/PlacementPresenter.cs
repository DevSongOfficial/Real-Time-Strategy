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

public enum PlacementResult
{
    Success,
    InvalidBuildingData,
    GridOccupied,
    InsufficientResources,
    MissingEditorContext // Failed while editing map
}

public sealed class PlacementPresenter : IPlacementEvent
{
    private readonly IPlacementView placementView;
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

    public PlacementPresenter(IPlacementView placementView, BuildingFactory buildingFactory, GridSystem gridSystem, InputManager inputManager)
    {
        this.placementView = placementView;
        this.buildingFactory = buildingFactory;
        this.gridSystem = gridSystem;
        this.inputManager = inputManager;
    }

    public void Enter() { }

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

    public PlacementResult TryPlaceForEditor(TeamContext teamContext)
    {
        return TryPlace(buildingData, teamContext, snappedPosition, out var building, spendResources: false);
    }

    public PlacementResult TryPlace(TeamContext teamContext)
    {
        return TryPlace(buildingData, teamContext, snappedPosition, out var building);
    }

    public PlacementResult TryPlace(BuildingData buildingData, TeamContext teamContext, Vector3 position, out Building placed, bool spendResources = true)
    {
        placed = null;

        if (buildingData == null)
            return PlacementResult.InvalidBuildingData;

        // Check on Grid.
        Vector2Int cellPosition = gridSystem.WorldToCell(position).ToVector2Int();
        Vector2Int cellSize = buildingData.CellSize;
        if (!gridSystem.CanPlace(cellPosition, cellSize))
            return PlacementResult.GridOccupied;

        // Check resource requirements and spend resources.
        if(spendResources)
        {
            if (!teamContext.ResourceBank.CanBuild(buildingData))
                return PlacementResult.InsufficientResources;

            teamContext.ResourceBank.SpendResource(ResourceType.Gold, buildingData.GoldRequired);
            teamContext.ResourceBank.SpendResource(ResourceType.Wood, buildingData.WoodRequired);
        }

        // Add to Grid.
        gridSystem.Occupy(cellPosition, cellSize);

        placementMode = PlacementMode.Idle;

        // Create & setup building.
        Building building = buildingFactory.Create(buildingData, teamContext);
        building.SetPosition(position);
        building.SetCellPosition(cellPosition);
        building.OnDestroyed += OnBuildingDestroyed;
        building.OnDestroyed += OnDestructed;
        building.OnDestructionRequested += OnBuildingDeconstructionRequested;
        placed = building;

        placementView.ToggleUIPreview(false);
        placementView.ToggleBuildingPreview(false);

        buildingData = null;

        OnPlacementRequested?.Invoke(building);

        return PlacementResult.Success;
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

    private void OnDestructed(Building building)
    {
        gridSystem.Release(building.GetCellPosition(), building.GetData().CellSize);
    }
}