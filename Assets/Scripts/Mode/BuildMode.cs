using System;
using BuildingSystem;
using CustomResourceManagement;
using UnityEngine;
using UnityEngine.UI;

public sealed class BuildMode : ModeBase
{
    private readonly InputManager inputManager;
    private readonly PlacementPresenter presenter;
    private readonly TeamContext teamContext;

    public BuildMode(TeamContext teamContext, InputManager inputManager, PlacementPresenter presenter)
    {
        this.teamContext = teamContext;
        this.inputManager = inputManager;
        this.presenter = presenter;
    }

    public override void Enter()
    {
        presenter.Enter();
    }

    public override void Exit()
    {
        presenter.Exit();
    }

    public override void Update()
    {
        var mousePosition = inputManager.GetMousePositionOnGround();
        presenter.UpdatePreview(mousePosition);
    }

    public override void HandleInput()
    {
        if (!inputManager.IsPointerInClickableArea()) return;

        if (inputManager.GetMouseButtonDown(0))
            TryPlace();

        if (inputManager.GetMouseButtonDown(1))
            presenter.Cancel();
    }

    // TODO: Replace debug logs with UI feedback
    private void TryPlace()
    {
        var result = presenter.TryPlace(teamContext);

        switch (result)
        {
            case PlacementResult.Success:
            case PlacementResult.InvalidBuildingData:
                return;
            case PlacementResult.GridOccupied:
                Debug.Log("Grid Occupied");
                break;
            case PlacementResult.InsufficientResources:
                Debug.Log("Resource Needed");
                break;
        }
    }
}