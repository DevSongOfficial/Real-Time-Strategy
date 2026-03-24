using System;
using BuildingSystem;
using CustomResourceManagement;
using UnityEngine;
using UnityEngine.UI;

public sealed class BuildMode : ModeBase
{
    public event Action<ITarget> OnBuildingPlaced;
    public event Action OnPlacementCanceled;


    private readonly InputManager inputManager;
    private readonly PlacementPresenter presenter;
    private TeamContext teamContext;

    private readonly bool useEditorPlacement;

    public BuildMode(TeamContext teamContext, InputManager inputManager, PlacementPresenter presenter, bool useEditorPlacement = false)
    {
        this.teamContext = teamContext;
        this.inputManager = inputManager;
        this.presenter = presenter;

        this.useEditorPlacement = useEditorPlacement;
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
            TryCancel();
    }

    public void ChangeTeamContext(TeamContext teamContext)
    {
        this.teamContext = teamContext;
    }

    // TODO: Replace debug logs with UI feedback
    private void TryPlace()
    {
        PlacementResult result;
        Building placed;
        if (useEditorPlacement) result = presenter.TryPlaceForEditor(teamContext, out placed);
        else                    result = presenter.TryPlace(teamContext, out placed);

        switch (result)
        {
            case PlacementResult.Success:
                OnBuildingPlaced?.Invoke(placed);
                break;
            case PlacementResult.InvalidBuildingData:
                break;
            case PlacementResult.GridOccupied:
                Debug.Log("Grid Occupied");
                break;
            case PlacementResult.InsufficientResources:
                Debug.Log("Resource Needed");
                break;
        }
    }

    private void TryCancel()
    {
        if (presenter.Cancel()) 
            OnPlacementCanceled?.Invoke();
    }
}