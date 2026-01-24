using UnityEngine;

public sealed class SelectTargetMode : ModeBase
{
    private readonly SelectionHandler selectionHandler;
    private readonly InputManager inputManager;

    public SelectTargetMode(InputManager inputManager, SelectionHandler selectionHandler)
    {
        this.selectionHandler = selectionHandler;
        this.inputManager = inputManager;
    }

    public override void Enter()
    {
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
    }

    public override void HandleInput()
    {
        if (!inputManager.IsPointerInClickableArea())
            return;

        if (inputManager.GetMouseButtonDown(0))
            selectionHandler.SelectTarget(inputManager.GetMousePosition());
    }
}