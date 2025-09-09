using System;
using BuildingSystem;
using CustomResourceManagement;
using UnityEngine;
using UnityEngine.UI;

public sealed class BuildMode : ModeBase
{
    private readonly InputManager inputManager;
    private readonly PlacementPresenter presenter;

    public BuildMode(InputManager inputManager, PlacementSystem placementSystem)
    {
        this.inputManager = inputManager;
        presenter = new PlacementPresenter(placementSystem);
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
        var mousePosition = inputManager.GetSelectedMapPosition();
        presenter.UpdatePreview(mousePosition);
    }

    public override void HandleInput()
    {
        if (inputManager.IsPointerOverUI()) return;

        if (inputManager.GetMouseButtonDown(0))
            presenter.Place();

        if (inputManager.GetMouseButtonDown(1))
        {
            presenter.Cancel();

        }
    }
}