using System;
using BuildingSystem;
using CustomResourceManagement;
using UnityEngine;
using UnityEngine.UI;

public sealed class BuildMode : ModeBase
{
    private readonly InputManager inputManager;
    private readonly PlacementPresenter presenter;

    public BuildMode(InputManager inputManager, PlacementPresenter presenter)
    {
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
            presenter.Place();

        if (inputManager.GetMouseButtonDown(1))
            presenter.Cancel();
    }
}