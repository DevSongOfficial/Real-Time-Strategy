using System;
using UnityEngine;

public class SelectMode : ModeBase
{
    private InputManager inputManager;
    private PlacementPresenter presenter;
    private Camera camera;

    private EditorEntityHandler entityHandler;

    public event Action<Building> OnBuildingSelected;

    public SelectMode(InputManager inputManager, PlacementPresenter presenter, Camera camera)
    {
        this.inputManager = inputManager;
        this.presenter = presenter;
        this.camera = camera;

        entityHandler = new EditorEntityHandler(camera);
    }

    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        
    }

    public override void HandleInput()
    {
        if (!inputManager.IsPointerInClickableArea())
            return;

        if (inputManager.GetMouseButtonDown(0))
            TrySelectEntity(inputManager.GetMousePositionOnCanvas());
    }

    public override void Update() { }

    private void TrySelectEntity(Vector2 screenPosition)
    {
        var entity = entityHandler.SelectEntity(inputManager.GetMousePositionOnCanvas());
        if (entity == null) return;

        if (entity is Building building)
        {
            entityHandler.Remove(building);
            OnBuildingSelected?.Invoke(building);
        }
    }
}
