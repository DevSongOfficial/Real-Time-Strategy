using System;
using BuildingSystem;
using CustomResourceManagement;
using UnityEngine;

public sealed class BuildMode : ModeBase
{
    private readonly InputManager inputManager;
    private readonly PlacementSystem placementSystem;

    public BuildMode(InputManager inputManager, PlacementSystem placementSystem)
    {
        this.inputManager = inputManager;
        this.placementSystem = placementSystem;
    }

    public override void Enter()
    {
        placementSystem.EnablePreview(true);
    }

    public override void Exit()
    {
        placementSystem.EnablePreview(false);
    }

    public override void Update()
    {
        var mousePosition = inputManager.GetSelectedMapPosition();
        placementSystem.UpdatePreview(mousePosition);
    }

    public override void HandleInput()
    {
        if (inputManager.IsPointerOverUI()) return;

        if (Input.GetMouseButtonDown(0))
            placementSystem.PlaceBuilding(/* building prefab */);

        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
        {
            // Transition to Normal Mode.

        }
    }
}