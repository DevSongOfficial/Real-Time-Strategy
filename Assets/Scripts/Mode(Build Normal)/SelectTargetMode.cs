using UnityEngine;

public sealed class SelectTargetMode : ModeBase
{
    private readonly SelectionHandler selectionHandler;
    private readonly InputManager inputManager;
    private readonly Transform mousePositionIndicator;

    public SelectTargetMode(InputManager inputManager, SelectionHandler selectionHandler, Transform mousePositionIndicator)
    {
        this.selectionHandler = selectionHandler;
        this.inputManager = inputManager;
        this.mousePositionIndicator = mousePositionIndicator;
    }

    public override void Enter()
    {
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
        mousePositionIndicator.position = inputManager.GetMousePositionOnGround();
    }

    public override void HandleInput()
    {
        if (!inputManager.IsPointerInClickableArea())
            return;

        if (inputManager.GetMouseButtonDown(0))
            selectionHandler.SelectTarget(inputManager.GetMousePositionOnCanvas());
            // Swith Mode
    }
}